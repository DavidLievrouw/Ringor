using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Models.Links;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Claim = Dalion.Ringor.Api.Models.Claim;

namespace Dalion.Ringor.Api.Controllers {
    public class UserInfoControllerTests {
        private readonly IClaimLinksCreatorFactory _claimLinksCreatorFactory;
        private readonly UserInfoController _sut;
        private readonly IUserInfoResponseLinksCreatorFactory _userInfoResponseLinksCreatorFactory;

        public UserInfoControllerTests() {
            FakeFactory.Create(out _userInfoResponseLinksCreatorFactory, out _claimLinksCreatorFactory);
            _sut = new UserInfoController(_userInfoResponseLinksCreatorFactory, _claimLinksCreatorFactory);
        }

        public class GetUserClaims : UserInfoControllerTests {
            private readonly Hyperlink<UserInfoResponseHyperlinkType>[] _userInfoResponseLinks;
            private readonly Hyperlink<ClaimHyperlinkType>[] _claimLinks;

            public GetUserClaims() {
                var httpContext = new DefaultHttpContext();
                var claimsIdentity = new ClaimsIdentity(new[] {
                    new System.Security.Claims.Claim("c1", "v1"),
                    new System.Security.Claims.Claim("c2", "v2")
                });
                httpContext.User = new ClaimsPrincipal(claimsIdentity);
                _sut.ControllerContext = new ControllerContext {
                    HttpContext = httpContext
                };

                var userInfoResponseLinksCreator = A.Fake<ILinksCreator<UserInfoResponse>>();
                A.CallTo(() => _userInfoResponseLinksCreatorFactory.Create())
                    .Returns(userInfoResponseLinksCreator);
                _userInfoResponseLinks = new[] {
                    new Hyperlink<UserInfoResponseHyperlinkType>(HttpMethod.Get, "https://dalion.eu/testing/api/userinfo", UserInfoResponseHyperlinkType.Self),
                    new Hyperlink<UserInfoResponseHyperlinkType>(HttpMethod.Get, "https://dalion.eu/testing/api", UserInfoResponseHyperlinkType.GetApiRoot)
                };
                A.CallTo(() => userInfoResponseLinksCreator.CreateLinksFor(A<UserInfoResponse>._))
                    .Invokes(call => {
                        var response = call.GetArgument<UserInfoResponse>(0);
                        response.Links = _userInfoResponseLinks;
                    });

                var claimLinksCreator = A.Fake<ILinksCreator<Claim>>();
                A.CallTo(() => _claimLinksCreatorFactory.Create())
                    .Returns(claimLinksCreator);
                _claimLinks = new[] {
                    new Hyperlink<ClaimHyperlinkType>(HttpMethod.Get, "https://dalion.eu/testing/api/userinfo", ClaimHyperlinkType.GetUserInfo)
                };
                A.CallTo(() => claimLinksCreator.CreateLinksFor(A<Claim>._))
                    .Invokes(call => {
                        var response = call.GetArgument<Claim>(0);
                        response.Links = _claimLinks;
                    });
            }

            [Fact]
            public async Task ReturnsOkWithUserClaims() {
                var actual = await _sut.GetUserClaims();

                actual.Should().BeOfType<OkObjectResult>();
                var expectedPayload = new UserInfoResponse {
                    Claims = new[] {
                        new Claim {Type = "c1", Value = "v1", Links = _claimLinks},
                        new Claim {Type = "c2", Value = "v2", Links = _claimLinks}
                    },
                    Links = _userInfoResponseLinks
                };
                actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedPayload);
            }
        }

        public class GetUserClaimsByType : UserInfoControllerTests {
            private readonly Hyperlink<ClaimHyperlinkType>[] _links;

            public GetUserClaimsByType() {
                var httpContext = new DefaultHttpContext();
                var claimsIdentity = new ClaimsIdentity(new[] {
                    new System.Security.Claims.Claim("c1", "v1"),
                    new System.Security.Claims.Claim("c2", "v2"),
                    new System.Security.Claims.Claim("c2", "v3")
                });
                httpContext.User = new ClaimsPrincipal(claimsIdentity);
                _sut.ControllerContext = new ControllerContext {
                    HttpContext = httpContext
                };

                var linksCreator = A.Fake<ILinksCreator<Claim>>();
                A.CallTo(() => _claimLinksCreatorFactory.Create())
                    .Returns(linksCreator);
                _links = new[] {
                    new Hyperlink<ClaimHyperlinkType>(HttpMethod.Get, "https://dalion.eu/testing/api/userinfo", ClaimHyperlinkType.GetUserInfo)
                };
                A.CallTo(() => linksCreator.CreateLinksFor(A<Claim>._))
                    .Invokes(call => {
                        var response = call.GetArgument<Claim>(0);
                        response.Links = _links;
                    });
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public async Task GivenNullOrEmptyClaimType_ReturnsNotFound(string invalidClaimType) {
                var actual = await _sut.GetUserClaimsByType(invalidClaimType);
                actual.Should().BeOfType<NotFoundResult>();
            }

            [Fact]
            public async Task WhenUserDoesNotHaveRequestedClaimType_ReturnsNotFound() {
                var actual = await _sut.GetUserClaimsByType("IDontExist");
                actual.Should().BeOfType<NotFoundResult>();
            }

            [Fact]
            public async Task WhenUserHasOneClaimOfType_ReturnsCollectionWithOneClaim() {
                var actual = await _sut.GetUserClaimsByType("c1");

                actual.Should().BeOfType<OkObjectResult>();
                var actualPayload = actual.As<OkObjectResult>().Value;
                var expectedPayload = new[] {
                    new Claim {Type = "c1", Value = "v1", Links = _links}
                };
                actualPayload.Should().BeEquivalentTo(expectedPayload);
            }

            [Fact]
            public async Task WhenUserHasMultipleClaimsOfType_ReturnsCollectionWithAllClaims() {
                var actual = await _sut.GetUserClaimsByType("c2");

                actual.Should().BeOfType<OkObjectResult>();
                var actualPayload = actual.As<OkObjectResult>().Value;
                var expectedPayload = new[] {
                    new Claim {Type = "c2", Value = "v2", Links = _links},
                    new Claim {Type = "c2", Value = "v3", Links = _links}
                };
                actualPayload.Should().BeEquivalentTo(expectedPayload);
            }

            [Fact]
            public async Task UrlDecodesClaimType() {
                _sut.ControllerContext.HttpContext.User.Identities.First()
                    .AddClaim(new System.Security.Claims.Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "test@dalion.eu"));

                var actual = await _sut.GetUserClaimsByType("http:%2F%2Fschemas.xmlsoap.org%2Fws%2F2005%2F05%2Fidentity%2Fclaims%2Femailaddress");

                actual.Should().BeOfType<OkObjectResult>();
                var actualPayload = actual.As<OkObjectResult>().Value;
                var expectedPayload = new[] {
                    new Claim {Type = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", Value = "test@dalion.eu", Links = _links}
                };
                actualPayload.Should().BeEquivalentTo(expectedPayload);
            }
        }
    }
}