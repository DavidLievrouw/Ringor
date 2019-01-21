using System;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class UserInfoResponseLinksCreatorTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly UserInfoResponseLinksCreator _sut;

        public UserInfoResponseLinksCreatorTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new UserInfoResponseLinksCreator(_hyperlinkFactory);
        }

        public class CreateLinksFor : UserInfoResponseLinksCreatorTests {
            private readonly UserInfoResponse _userInfoResponse;

            public CreateLinksFor() {
                _userInfoResponse = new UserInfoResponse {
                    Claims = new[] {
                        new Claim {Type = "c1", Value = "v1"},
                        new Claim {Type = "c2", Value = "v2"}
                    }
                };
                A.CallTo(() => _hyperlinkFactory.Create(A<HttpMethod>._, A<string>._, A<UserInfoResponseHyperlinkType>._))
                    .ReturnsLazily(call => {
                        var method = call.GetArgument<HttpMethod>(0);
                        var relativeUrl = call.GetArgument<string>(1);
                        var rel = call.GetArgument<UserInfoResponseHyperlinkType>(2);
                        return new Hyperlink<UserInfoResponseHyperlinkType>(method, $"https://recomatics.com/testing{relativeUrl}", rel);
                    });
            }

            [Fact]
            public void GivenNullModel_DoesNotThrow() {
                Action act = () => _sut.CreateLinksFor(null);
                act.Should().NotThrow();
            }

            [Fact]
            public async Task AddsExpectedLinksToModel() {
                await _sut.CreateLinksFor(_userInfoResponse);
                var expected = new UserInfoResponse {
                    Claims = new[] {
                        new Claim {Type = "c1", Value = "v1"},
                        new Claim {Type = "c2", Value = "v2"}
                    },
                    Links = new[] {
                        new Hyperlink<UserInfoResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api/userinfo", UserInfoResponseHyperlinkType.Self),
                        new Hyperlink<UserInfoResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api", UserInfoResponseHyperlinkType.GetApiRoot)
                    }
                };
                var differences = _userInfoResponse.CompareTo(expected);
                differences.AreEqual.Should().BeTrue(because: differences.DifferencesString);
            }
        }
    }
}