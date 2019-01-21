using System;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApiHomeResponseLinksCreatorTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly ApiHomeResponseLinksCreator _sut;

        public ApiHomeResponseLinksCreatorTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new ApiHomeResponseLinksCreator(_hyperlinkFactory);
        }

        public class CreateLinksFor : ApiHomeResponseLinksCreatorTests {
            private readonly ApiHomeResponse _apiHomeResponse;

            public CreateLinksFor() {
                _apiHomeResponse = new ApiHomeResponse {
                    ApplicationInfo = new ApplicationInfo {
                        Version = "1.2.3",
                        Message = "For unit tests",
                        UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                            SiteUrl = "https://www.dalion.eu/",
                            AppUrl = "Ringor"
                        }
                    }
                };
                A.CallTo(() => _hyperlinkFactory.Create(A<HttpMethod>._, A<string>._, A<ApiHomeResponseHyperlinkType>._))
                    .ReturnsLazily(call => {
                        var method = call.GetArgument<HttpMethod>(0);
                        var relativeUrl = call.GetArgument<string>(1);
                        var rel = call.GetArgument<ApiHomeResponseHyperlinkType>(2);
                        return new Hyperlink<ApiHomeResponseHyperlinkType>(method, $"https://recomatics.com/testing{relativeUrl}", rel);
                    });
            }

            [Fact]
            public void GivenNullModel_DoesNotThrow() {
                Action act = () => _sut.CreateLinksFor(null);
                act.Should().NotThrow();
            }

            [Fact]
            public async Task AddsExpectedLinksToModel() {
                await _sut.CreateLinksFor(_apiHomeResponse);
                var expected = new ApiHomeResponse {
                    ApplicationInfo = new ApplicationInfo {
                        Version = "1.2.3",
                        Message = "For unit tests",
                        UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                            SiteUrl = "https://www.dalion.eu/",
                            AppUrl = "Ringor"
                        }
                    },
                    Links = new[] {
                        new Hyperlink<ApiHomeResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api", ApiHomeResponseHyperlinkType.Self),
                        new Hyperlink<ApiHomeResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api/userinfo", ApiHomeResponseHyperlinkType.GetUserInfo)
                    }
                };
                var differences = _apiHomeResponse.CompareTo(expected);
                differences.AreEqual.Should().BeTrue(because: differences.DifferencesString);
            }
        }
    }
}