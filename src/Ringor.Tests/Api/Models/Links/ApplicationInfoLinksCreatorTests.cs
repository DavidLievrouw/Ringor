using System;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationInfoLinksCreatorTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly ApplicationInfoLinksCreator _sut;

        public ApplicationInfoLinksCreatorTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new ApplicationInfoLinksCreator(_hyperlinkFactory);
        }

        public class CreateLinksFor : ApplicationInfoLinksCreatorTests {
            private readonly ApplicationInfo _applicationInfo;

            public CreateLinksFor() {
                _applicationInfo = new ApplicationInfo {
                    Version = "1.2.3",
                    Message = "For unit tests",
                    UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                        SiteUrl = "https://www.dalion.eu/",
                        AppUrl = "Ringor"
                    }
                };
                A.CallTo(() => _hyperlinkFactory.Create(A<HttpMethod>._, A<string>._, A<ApplicationInfoHyperlinkType>._))
                    .ReturnsLazily(call => {
                        var method = call.GetArgument<HttpMethod>(0);
                        var relativeUrl = call.GetArgument<string>(1);
                        var rel = call.GetArgument<ApplicationInfoHyperlinkType>(2);
                        return new Hyperlink<ApplicationInfoHyperlinkType>(method, $"https://recomatics.com/testing{relativeUrl}", rel);
                    });
            }

            [Fact]
            public void GivenNullModel_DoesNotThrow() {
                Action act = () => _sut.CreateLinksFor(null);
                act.Should().NotThrow();
            }

            [Fact]
            public async Task AddsExpectedLinksToModel() {
                await _sut.CreateLinksFor(_applicationInfo);
                var expected = new ApplicationInfo {
                    Version = "1.2.3",
                    Message = "For unit tests",
                    UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                        SiteUrl = "https://www.dalion.eu/",
                        AppUrl = "Ringor"
                    },
                    Links = new[] {
                        new Hyperlink<ApplicationInfoHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api", ApplicationInfoHyperlinkType.Self),
                        new Hyperlink<ApplicationInfoHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api/userinfo", ApplicationInfoHyperlinkType.GetUserInfo)
                    }
                };
                var differences = _applicationInfo.CompareTo(expected);
                differences.AreEqual.Should().BeTrue(because: differences.DifferencesString);
            }
        }
    }
}