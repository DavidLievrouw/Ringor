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
                    HttpContext = new DefaultHttpContext()
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

        public class NotFound404 : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;
            private readonly IStatusCodeReExecuteFeature _feature;
            private readonly string _suffix;

            public NotFound404() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
                FakeFactory.Create(out _feature);
                _sut.HttpContext.Features.Set(_feature);
                _suffix = "";
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.NotFound404(_suffix);
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }

            [Fact]
            public void AddsOriginalPathToViewData() {
                A.CallTo(() => _feature.OriginalPath)
                    .Returns("/foo/bar");

                var actual = _sut.NotFound404(_suffix);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPath", "/foo/bar");
            }

            [Fact]
            public void AddsOriginalPathBaseToViewData() {
                A.CallTo(() => _feature.OriginalPathBase)
                    .Returns("/RingorApp");

                var actual = _sut.NotFound404(_suffix);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPathBase", "/RingorApp");
            }

            [Fact]
            public void AddsOriginalQueryStringToViewData() {
                A.CallTo(() => _feature.OriginalQueryString)
                    .Returns("?debug=true");

                var actual = _sut.NotFound404(_suffix);

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorQueryString", "?debug=true");
            }

            [Fact]
            public void WhenFeatureIsNotAvailable_DoesNotThrow_DoesNotAddOriginalPathToViewData() {
                _sut.ControllerContext.HttpContext = new DefaultHttpContext();

                var actual = _sut.NotFound404(_suffix);

                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPath");
            }

            [Theory]
            [InlineData("/error/404")]
            [InlineData("/error/404.14")]
            [InlineData("/error/404.xx")]
            [InlineData("/error/404?debug=true")]
            public async Task CatchesRoutesForNotFoundStatusCodes(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => IsCallToNotFoundErrorEndpoint(_));
            }

            private static bool IsCallToNotFoundErrorEndpoint(HttpResponseMessage response) {
                return response.Headers.TryGetValues("Dalion-ResponseType", out var values) &&
                       values.Contains("NotFoundError") &&
                       response.IsSuccessStatusCode;
            }
        }

        public class OtherError : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;
            private readonly string _url;

            public OtherError() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
                _url = "400";
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.OtherError(_url);
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }

            [Theory]
            [InlineData("/error/100")]
            [InlineData("/error/200")]
            [InlineData("/error/400")]
            [InlineData("/error/400.14")]
            [InlineData("/error/somethingelse")]
            [InlineData("/error/400?debug=true")]
            [InlineData("/error/somethingelse?debug=true")]
            public async Task CatchesAllNon404Errors(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => IsCallToCatchAllErrorEndpoint(_));
            }

            private static bool IsCallToCatchAllErrorEndpoint(HttpResponseMessage response) {
                return response.Headers.TryGetValues("Dalion-ResponseType", out var values) &&
                       values.Contains("CatchAllError") &&
                       response.IsSuccessStatusCode;
            }
        }
    }
}