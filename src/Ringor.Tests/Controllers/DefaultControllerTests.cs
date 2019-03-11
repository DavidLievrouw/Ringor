using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dalion.Ringor.Startup;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Dalion.Ringor.Controllers {
    public class DefaultControllerTests {
        private readonly DefaultController _sut;
        private readonly string _url;

        public DefaultControllerTests() {
            _sut = new DefaultController();
            _url = "";
        }

        public class Index : DefaultControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;

            public Index() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.Index(_url);
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }

            [Theory]
            [InlineData("")]
            [InlineData("/")]
            [InlineData("/login")]
            [InlineData("/login/")]
            [InlineData("/Login")]
            [InlineData("/Login/")]
            [InlineData("/login?debug=true")]
            [InlineData("/login/?debug=true")]
            [InlineData("/swaggerui")]
            [InlineData("/swaggerui/")]
            [InlineData("/Swaggerui/")]
            [InlineData("/Swaggerui")]
            [InlineData("/swaggerui?debug=true")]
            [InlineData("/swaggerui/?debug=true")]
            [InlineData("/apinav")]
            [InlineData("/apinav/")]
            [InlineData("/ApiNav/")]
            [InlineData("/ApiNav")]
            [InlineData("/apinav?debug=true")]
            [InlineData("/apinav/?debug=true")]
            [InlineData("/home")]
            [InlineData("/home/")]
            [InlineData("/home?debug=true")]
            [InlineData("/home/?debug=true")]
            public async Task MatchesWildcardRoutes(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => IsCallToSPA(_));
            }

            [Theory]
            [InlineData("/login/page")]
            [InlineData("/login/page/segment")]
            [InlineData("/login/page?debug=true")]
            [InlineData("/swaggerui/page")]
            [InlineData("/swaggerui/page/segment")]
            [InlineData("/swaggerui/page?debug=true")]
            public async Task DoesNotMatchUrlsWithMultipleSegments(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
            }

            [Theory]
            [InlineData("/api")]
            [InlineData("/api/")]
            [InlineData("/api/request")]
            [InlineData("/api/r1/r2")]
            [InlineData("/api?debug=true")]
            [InlineData("/Api")]
            [InlineData("/Api/")]
            [InlineData("/Api/page")]
            [InlineData("/Api?debug=true")]
            public async Task DoesNotMatchApiUrls(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
            }

            [Theory]
            [InlineData("/swagger")]
            [InlineData("/swagger/")]
            [InlineData("/swagger/page")]
            [InlineData("/swagger/page/page2")]
            [InlineData("/swagger?debug=true")]
            [InlineData("/Swagger")]
            [InlineData("/Swagger/")]
            [InlineData("/Swagger/page")]
            [InlineData("/Swagger?debug=true")]
            public async Task DoesNotMatchSwaggerUrls(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
            }

            [Theory]
            [InlineData("/js-bundle.js")]
            [InlineData("/js-bundle.png")]
            [InlineData("/favicon.ico")]
            [InlineData("/App/js-bundle.js")]
            [InlineData("/App/assets/js-bundle.js")]
            [InlineData("/subsite/js-bundle.png")]
            [InlineData("/app/favicon.ico")]
            public async Task DoesNotMatchRouteForStaticFiles(string url) {
                var response = await _client.GetAsync(url);
                response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
            }

            private bool IsCallToSPA(HttpResponseMessage response) {
                return
                    response.Headers.TryGetValues("Dalion-ResponseType", out var values) &&
                    values.Contains("SPA-View") &&
                    response.IsSuccessStatusCode;
            }
        }
    }
}