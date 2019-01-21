using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Models.Links;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Claim = System.Security.Claims.Claim;

namespace Dalion.Ringor.Api.Controllers {
    public class UserInfoControllerTests {
        private readonly IUserInfoResponseLinksCreatorFactory _userInfoResponseLinksCreatorFactory;
        private readonly UserInfoController _sut;

        public UserInfoControllerTests() {
            FakeFactory.Create(out _userInfoResponseLinksCreatorFactory);
            _sut = new UserInfoController(_userInfoResponseLinksCreatorFactory);
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
                var linksCreator = A.Fake<ILinksCreator<UserInfoResponse>>();
                A.CallTo(() => _userInfoResponseLinksCreatorFactory.Create())
                    .Returns(linksCreator);
                var links = new[] {
                    new Hyperlink<UserInfoResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api/userinfo", UserInfoResponseHyperlinkType.Self),
                    new Hyperlink<UserInfoResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api", UserInfoResponseHyperlinkType.GetApiRoot)
                };
                A.CallTo(() => linksCreator.CreateLinksFor(A<UserInfoResponse>._))
                    .Invokes(call => {
                        var response = call.GetArgument<UserInfoResponse>(0);
                        response.Links = links;
                    });

                var actual = _sut.GetUserClaims();

                actual.Should().BeOfType<OkObjectResult>();
                var expectedPayload = new UserInfoResponse {
                    Claims = new[] {
                        new Models.Claim {Type = "c1", Value = "v1"},
                        new Models.Claim {Type = "c2", Value = "v2"}
                    },
                    Links = links
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
                    new Models.Claim {Type = "c1", Value = "v1"}
                };
                actualPayload.Should().BeEquivalentTo(expectedPayload);
            }

            [Fact]
            public void WhenUserHasMultipleClaimsOfType_ReturnsCollectionWithAllClaims() {
                var actual = _sut.GetUserClaimsByType("c2");
                actual.Should().BeOfType<OkObjectResult>();
                var actualPayload = actual.As<OkObjectResult>().Value;
                var expectedPayload = new[] {
                    new Models.Claim {Type = "c2", Value = "v2"},
                    new Models.Claim {Type = "c2", Value = "v3"}
                };
                actualPayload.Should().BeEquivalentTo(expectedPayload);
            }

            [Fact]
            public void UrlDecodesClaimType() {
                _sut.ControllerContext.HttpContext.User.Identities.First()
                    .AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "test@recomatics.com"));
                var actual = _sut.GetUserClaimsByType("http:%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Femailaddress");

                actual.Should().BeOfType<OkObjectResult>();
                var actualPayload = actual.As<OkObjectResult>().Value;
                var expectedPayload = new[] {
                    new Models.Claim {Type = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", Value = "test@recomatics.com"}
                };
                actualPayload.Should().BeEquivalentTo(expectedPayload);
            }
        }
    }
}