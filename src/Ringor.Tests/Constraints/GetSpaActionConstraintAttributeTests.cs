using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Dalion.Ringor.Constraints {
    public class GetSpaActionConstraintAttributeTests {
        private readonly GetSpaActionConstraintAttribute _sut;

        public GetSpaActionConstraintAttributeTests() {
            _sut = new GetSpaActionConstraintAttribute();
        }

        public class Order : GetSpaActionConstraintAttributeTests {
            [Fact]
            public void ReturnsMinusTen() {
                _sut.Order.Should().Be(-10);
            }
        }

        public class Accept : GetSpaActionConstraintAttributeTests {
            private readonly ActionConstraintContext _context;

            public Accept() {
                _context = new ActionConstraintContext {
                    RouteContext = new RouteContext(new DefaultHttpContext {
                        Request = { Method = HttpMethods.Get}
                    })
                };
            }


            [Theory]
            [InlineData("")]
            [InlineData("/")]
            [InlineData("/login")]
            [InlineData("/login/")]
            [InlineData("/Login")]
            [InlineData("/Login/")]
            [InlineData("/swaggerui")]
            [InlineData("/swaggerui/")]
            [InlineData("/Swaggerui/")]
            [InlineData("/Swaggerui")]
            [InlineData("/apinav")]
            [InlineData("/apinav/")]
            [InlineData("/ApiNav/")]
            [InlineData("/ApiNav")]
            [InlineData("/home")]
            [InlineData("/home/")]
            public void MatchesWildcardRoutes(string path) {
                _context.RouteContext.HttpContext.Request.Path = PathString.FromUriComponent(path);
                var actual = _sut.Accept(_context);
                actual.Should().BeTrue();
            }

            [Theory]
            [InlineData("/login/page")]
            [InlineData("/login/page/segment")]
            [InlineData("/swaggerui/page")]
            [InlineData("/swaggerui/page/segment")]
            public void DoesNotMatchUrlsWithMultipleSegments(string path) {
                _context.RouteContext.HttpContext.Request.Path = PathString.FromUriComponent(path);
                var actual = _sut.Accept(_context);
                actual.Should().BeFalse();
            }

            [Theory]
            [InlineData("/api")]
            [InlineData("/api/")]
            [InlineData("/api/request")]
            [InlineData("/api/r1/r2")]
            [InlineData("/Api")]
            [InlineData("/Api/")]
            [InlineData("/Api/page")]
            public void DoesNotMatchApiUrls(string path) {
                _context.RouteContext.HttpContext.Request.Path = PathString.FromUriComponent(path);
                var actual = _sut.Accept(_context);
                actual.Should().BeFalse();
            }

            [Theory]
            [InlineData("/swagger")]
            [InlineData("/swagger/")]
            [InlineData("/swagger/page")]
            [InlineData("/swagger/page/page2")]
            [InlineData("/Swagger")]
            [InlineData("/Swagger/")]
            [InlineData("/Swagger/page")]
            public void DoesNotMatchSwaggerUrls(string path) {
                _context.RouteContext.HttpContext.Request.Path = PathString.FromUriComponent(path);
                var actual = _sut.Accept(_context);
                actual.Should().BeFalse();
            }

            [Theory]
            [InlineData("/js-bundle.js")]
            [InlineData("/js-bundle.png")]
            [InlineData("/favicon.ico")]
            [InlineData("/App/js-bundle.js")]
            [InlineData("/App/assets/js-bundle.js")]
            [InlineData("/subsite/js-bundle.png")]
            [InlineData("/app/favicon.ico")]
            public void DoesNotMatchRouteForStaticFiles(string path) {
                _context.RouteContext.HttpContext.Request.Path = PathString.FromUriComponent(path);
                var actual = _sut.Accept(_context);
                actual.Should().BeFalse();
            }
        }
    }
}