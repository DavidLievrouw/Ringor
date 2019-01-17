using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Dalion.Ringor.Api.Controllers {
    public class UserInfoControllerTests {
        private readonly UserInfoController _sut;

        public UserInfoControllerTests() {
            _sut = new UserInfoController();
        }

        public class GetUserClaims : UserInfoControllerTests {
            public GetUserClaims() {
                var httpContext = new DefaultHttpContext();
                var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim("c1", "v1"),
                    new Claim("c2", "v2")
                });
                httpContext.User = new ClaimsPrincipal(claimsIdentity);
                _sut.ControllerContext = new ControllerContext {
                    HttpContext = httpContext
                };
            }

            [Fact]
            public void ReturnsOkWithUserClaims() {
                var actual = _sut.GetUserClaims();
                actual.Should().BeOfType<OkObjectResult>();
                var expectedPayload = new[] {
                    new {Type = "c1", Value = "v1"},
                    new {Type = "c2", Value = "v2"}
                };
                actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedPayload);
            }
        }

        public class GetUserClaimsByType : UserInfoControllerTests {
            public GetUserClaimsByType() {
                var httpContext = new DefaultHttpContext();
                var claimsIdentity = new ClaimsIdentity(new[] {
                    new Claim("c1", "v1"),
                    new Claim("c2", "v2"),
                    new Claim("c2", "v3")
                });
                httpContext.User = new ClaimsPrincipal(claimsIdentity);
                _sut.ControllerContext = new ControllerContext {
                    HttpContext = httpContext
                };
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void GivenNullOrEmptyClaimType_ReturnsNotFound(string invalidClaimType) {
                var actual = _sut.GetUserClaimsByType(invalidClaimType);
                actual.Should().BeOfType<NotFoundResult>();
            }

            [Fact]
            public void WhenUserDoesNotHaveRequestedClaimType_ReturnsNotFound() {
                var actual = _sut.GetUserClaimsByType("IDontExist");
                actual.Should().BeOfType<NotFoundResult>();
            }

            [Fact]
            public void WhenUserHasOneClaimOfType_ReturnsCollectionWithOneClaim() {
                var actual = _sut.GetUserClaimsByType("c1");
                actual.Should().BeOfType<OkObjectResult>();
                var actualPayload = actual.As<OkObjectResult>().Value;
                var expectedPayload = new[] {
                    new {Type = "c1", Value = "v1"}
                };
                actualPayload.Should().BeEquivalentTo(expectedPayload);
            }

            [Fact]
            public void WhenUserHasMultipleClaimsOfType_ReturnsCollectionWithAllClaims() {
                var actual = _sut.GetUserClaimsByType("c2");
                actual.Should().BeOfType<OkObjectResult>();
                var actualPayload = actual.As<OkObjectResult>().Value;
                var expectedPayload = new[] {
                    new {Type = "c2", Value = "v2"},
                    new {Type = "c2", Value = "v3"}
                };
                actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedPayload);
            }
        }
    }
}