using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dalion.Ringor.Startup;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Dalion.Ringor.Controllers {
    public class ErrorControllerTests {
        private readonly ErrorController _sut;

        public ErrorControllerTests() {
            _sut = new ErrorController {
                ControllerContext = new ControllerContext {
                    HttpContext = new DefaultHttpContext {
                        Response = {StatusCode = 204}
                    }
                }
            };
        }

        public class InternalServerError : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;
            private readonly IExceptionHandlerPathFeature _feature;

            public InternalServerError() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
                FakeFactory.Create(out _feature);
                _sut.HttpContext.Features.Set(_feature);
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.InternalServerError();
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }

            [Fact]
            public void AddsErroneousPathToViewData() {
                A.CallTo(() => _feature.Path)
                    .Returns("/foo/bar");

                var actual = _sut.InternalServerError();

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPath", "/foo/bar");
            }

            [Fact]
            public void AddsErrorToViewData() {
                var cause = new InvalidOperationException("Epic failure");
                A.CallTo(() => _feature.Error)
                    .Returns(cause);

                var actual = _sut.InternalServerError();

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-Error", cause);
            }

            [Fact]
            public void WhenFeatureIsNotAvailable_DoesNotThrow_DoesNotAddErroneousPathOrErrorToViewData() {
                _sut.ControllerContext.HttpContext = new DefaultHttpContext();

                var actual = _sut.InternalServerError();

                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPath");
                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-Error");
            }

            [Theory]
            [InlineData("/error")]
            [InlineData("/error/")]
            [InlineData("/error?debug=true")]
            [InlineData("/error/?debug=true")]
            public async Task ReturnsServerErrorView(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => IsCallToServerErrorEndpoint(_));
            }

            private static bool IsCallToServerErrorEndpoint(HttpResponseMessage response) {
                return response.Headers.TryGetValues("Dalion-ResponseType", out var values) &&
                       values.Contains("ServerError") &&
                       response.IsSuccessStatusCode;
            }
        }

        public class CatchAllStatusCodes : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;
            private readonly IStatusCodeReExecuteFeature _feature;
            private readonly string _status;
            private readonly string _path;

            public CatchAllStatusCodes() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
                FakeFactory.Create(out _feature);
                _sut.HttpContext.Features.Set(_feature);
                _status = "401";
                _path = null;
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.CatchAllStatusCodes(_status, _path);
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }

            [Fact]
            public void AddsOriginalPathToViewData() {
                A.CallTo(() => _feature.OriginalPath)
                    .Returns("/foo/bar");

                var actual = _sut.CatchAllStatusCodes(_status, _path);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPath", "/foo/bar");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void WhenThereIsNoOriginalPath_DoesNotAddIt(string noValue) {
                A.CallTo(() => _feature.OriginalPath)
                    .Returns(noValue);

                var actual = _sut.CatchAllStatusCodes(_status, _path);

                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPath");
            }
            
            [Fact]
            public void WhenThereIsNoOriginalPathInFeature_ButItIsSpecifiedInQueryString_AddsToViewData() {
                A.CallTo(() => _feature.OriginalPath)
                    .Returns(null);

                var actual = _sut.CatchAllStatusCodes(_status, "/path/from/qs");

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPath", "/path/from/qs");
            }

            [Fact]
            public void WhenThereAreBothQueryStringPathAndFeaturePath_PrefersFeaturePath() {
                A.CallTo(() => _feature.OriginalPath)
                    .Returns("/foo/bar");

                var actual = _sut.CatchAllStatusCodes(_status, "/path/from/qs");

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPath", "/foo/bar");
            }

            [Fact]
            public void AddsOriginalPathBaseToViewData() {
                A.CallTo(() => _feature.OriginalPathBase)
                    .Returns("/RingorApp");

                var actual = _sut.CatchAllStatusCodes(_status, _path);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPathBase", "/RingorApp");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void WhenThereIsNoOriginalPathBase_DoesNotAddIt(string noValue) {
                A.CallTo(() => _feature.OriginalPathBase)
                    .Returns(noValue);

                var actual = _sut.CatchAllStatusCodes(_status, _path);

                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPathBase");
            }

            [Fact]
            public void AddsOriginalQueryStringToViewData() {
                A.CallTo(() => _feature.OriginalQueryString)
                    .Returns("?debug=true");

                var actual = _sut.CatchAllStatusCodes(_status, _path);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorQueryString", "?debug=true");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void WhenThereIsNoOriginalQueryString_DoesNotAddIt(string noValue) {
                A.CallTo(() => _feature.OriginalQueryString)
                    .Returns(noValue);

                var actual = _sut.CatchAllStatusCodes(_status, _path);

                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorQueryString");
            }

            [Theory]
            [InlineData("200", 200)]
            [InlineData("401", 401)]
            [InlineData("401.17", 401)]
            [InlineData("401-xx", 401)]
            [InlineData("4-a01-xx", 4)]
            public void AddsStatusCodeToViewData(string status, int expectedStatusCode) {
                var actual = _sut.CatchAllStatusCodes(status, _path);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorStatusCode", expectedStatusCode);
            }

            [Fact]
            public void AddsStatusCodeIsNotANumber_AddsReponseStatusCodeToView() {
                var actual = _sut.CatchAllStatusCodes("NaN", _path);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorStatusCode", _sut.ControllerContext.HttpContext.Response.StatusCode);
            }

            [Fact]
            public void WhenFeatureIsNotAvailable_DoesNotThrow_DoesNotAddOriginalRequestValuesToViewData() {
                _sut.ControllerContext.HttpContext = new DefaultHttpContext();

                var actual = _sut.CatchAllStatusCodes(_status, _path);

                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPath");
                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPathBase");
                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPathQueryString");
            }

            [Fact]
            public void WhenFeatureIsNotAvailable_ButQueryStringPathIs_OnlyAddsQueryStringPathToViewData() {
                _sut.ControllerContext.HttpContext = new DefaultHttpContext();

                var actual = _sut.CatchAllStatusCodes(_status, "/path/from/qs");

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPath", "/path/from/qs");
                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPathBase");
                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPathQueryString");
            }

            [Theory]
            [InlineData("/api", "401", 401)]
            [InlineData("/api/", "401", 401)]
            [InlineData("/Api", "401", 401)]
            [InlineData("/Api/", "401", 401)]
            [InlineData("/api/userinfo", "401", 401)]
            [InlineData("/Api/userinfo", "401", 401)]
            [InlineData("/api", "401.2", 401)]
            [InlineData("/api/", "401.2", 401)]
            [InlineData("/Api", "401.2", 401)]
            [InlineData("/Api/", "401.2", 401)]
            [InlineData("/api/userinfo", "401.2", 401)]
            [InlineData("/Api/userinfo", "401.2", 401)]
            public void ReturnJsonWithoutBodyWhenOriginalCallWasToApi(string originalPath, string status, int expectedStatusCode) {
                A.CallTo(() => _feature.OriginalPath)
                    .Returns(originalPath);

                var actual = _sut.CatchAllStatusCodes(status, _path);

                actual.Should().BeAssignableTo<StatusCodeResult>();
                actual.As<StatusCodeResult>().StatusCode.Should().Be(expectedStatusCode);
            }

            [Theory]
            [InlineData("404")]
            [InlineData("404.14")]
            [InlineData("404.xx")]
            public void ReturnsNotFoundViewFor404(string status) {
                var actual = _sut.CatchAllStatusCodes(status, _path);
                actual.As<ViewResult>().ViewName.Should().Be("NotFound");
            }

            [Theory]
            [InlineData("404")]
            [InlineData("404.14")]
            [InlineData("404.xx")]
            public void ReturnsNotFoundCodeFor404(string status) {
                var actual = _sut.CatchAllStatusCodes(status, _path);
                actual.As<ViewResult>().StatusCode.Should().Be(404);
            }

            [Theory]
            [InlineData("100")]
            [InlineData("200")]
            [InlineData("400")]
            [InlineData("400.14")]
            [InlineData("500")]
            [InlineData("503.2")]
            [InlineData("somethingelse")]
            public void ReturnsOtherErrorViewForNon404Codes(string status) {
                var actual = _sut.CatchAllStatusCodes(status, _path);
                actual.As<ViewResult>().ViewName.Should().Be("OtherError");
            }

            [Theory]
            [InlineData("100", 100)]
            [InlineData("200", 200)]
            [InlineData("400", 400)]
            [InlineData("400.14", 400)]
            [InlineData("500", 500)]
            [InlineData("503.2", 503)]
            [InlineData("somethingelse", 204)]
            public void ReturnsStatusCodeNon404Codes(string status, int expectedStatusCode) {
                var actual = _sut.CatchAllStatusCodes(status, _path);
                actual.As<ViewResult>().StatusCode.Should().Be(expectedStatusCode);
            }

            [Theory]
            [InlineData("/error/404")]
            [InlineData("/error/404.14")]
            [InlineData("/error/404.xx")]
            [InlineData("/error/404?debug=true")]
            [InlineData("/error/100")]
            [InlineData("/error/200")]
            [InlineData("/error/400")]
            [InlineData("/error/400.14")]
            [InlineData("/error/500")]
            [InlineData("/error/503.2")]
            [InlineData("/error/somethingelse")]
            [InlineData("/error/400?debug=true")]
            [InlineData("/error/somethingelse?debug=true")]
            public async Task CatchesRoutesForAllStatusCodes(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => IsCallToCatchAllStatusCodesEndpoint(_));
            }

            private static bool IsCallToCatchAllStatusCodesEndpoint(HttpResponseMessage response) {
                return response.Headers.TryGetValues("Dalion-ResponseType", out var values) &&
                       values.Contains("CatchAllError");
            }
        }
    }
}