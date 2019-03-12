using System;
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
        }

        public class NotFound404 : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;
            private readonly IStatusCodeReExecuteFeature _feature;

            public NotFound404() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
                FakeFactory.Create(out _feature);
                _sut.HttpContext.Features.Set(_feature);
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.NotFound404();
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }

            [Fact]
            public void AddsOriginalPathToViewData() {
                A.CallTo(() => _feature.OriginalPath)
                    .Returns("/foo/bar");

                var actual = _sut.NotFound404();

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPath", "/foo/bar");
            }

            [Fact]
            public void AddsOriginalPathBaseToViewData() {
                A.CallTo(() => _feature.OriginalPathBase)
                    .Returns("/RingorApp");

                var actual = _sut.NotFound404();

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorPathBase", "/RingorApp");
            }

            [Fact]
            public void AddsOriginalQueryStringToViewData() {
                A.CallTo(() => _feature.OriginalQueryString)
                    .Returns("?debug=true");

                var actual = _sut.NotFound404();

                actual.As<ViewResult>().ViewData.Should().Contain("Dalion-ErrorQueryString", "?debug=true");
            }

            [Fact]
            public void WhenFeatureIsNotAvailable_DoesNotThrow_DoesNotAddOriginalPathToViewData() {
                _sut.ControllerContext.HttpContext = new DefaultHttpContext();

                var actual = _sut.NotFound404();

                actual.As<ViewResult>().ViewData.Should().NotContainKey("Dalion-ErrorPath");
            }
        }

        public class OtherError : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;

            public OtherError() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.OtherError();
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
                response.Should().Match<HttpResponseMessage>(_ => IsCallToCatchAllEndpoint(_));
            }

            private static bool IsCallToCatchAllEndpoint(HttpResponseMessage response) {
                return response.IsSuccessStatusCode &&;
            }
        }
    }
}