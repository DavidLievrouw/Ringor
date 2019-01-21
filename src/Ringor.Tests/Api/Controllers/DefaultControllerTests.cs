using System.Net.Http;
using System.Threading.Tasks;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Models.Links;
using Dalion.Ringor.Api.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Dalion.Ringor.Api.Controllers {
    public class DefaultControllerTests {
        private readonly IApiHomeResponseLinksCreatorFactory _apiHomeResponseLinksCreatorFactory;
        private readonly IApplicationInfoProvider _applicationInfoProvider;
        private readonly DefaultController _sut;

        public DefaultControllerTests() {
            FakeFactory.Create(out _applicationInfoProvider, out _apiHomeResponseLinksCreatorFactory);
            _sut = new DefaultController(_applicationInfoProvider, _apiHomeResponseLinksCreatorFactory);
        }

        public class GetDefault : DefaultControllerTests {
            private readonly ApplicationInfo _applicationInfo;

            public GetDefault() {
                _applicationInfo = new ApplicationInfo {
                    Version = "1.2.3",
                    Message = "For unit tests",
                    UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                        SiteUrl = "https://www.dalion.eu/",
                        AppUrl = "Ringor"
                    }
                };
                A.CallTo(() => _applicationInfoProvider.Provide())
                    .Returns(_applicationInfo);
            }

            [Fact]
            public async Task ReturnsOkWithExpectedApiHomeResponse() {
                var linksCreator = A.Fake<ILinksCreator<ApiHomeResponse>>();
                A.CallTo(() => _apiHomeResponseLinksCreatorFactory.Create())
                    .Returns(linksCreator);
                var links = new[] {
                    new Hyperlink<ApiHomeResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api", ApiHomeResponseHyperlinkType.Self),
                    new Hyperlink<ApiHomeResponseHyperlinkType>(HttpMethod.Get, "https://recomatics.com/testing/api/userinfo", ApiHomeResponseHyperlinkType.GetUserInfo)
                };
                A.CallTo(() => linksCreator.CreateLinksFor(A<ApiHomeResponse>._))
                    .Invokes(call => {
                        var response = call.GetArgument<ApiHomeResponse>(0);
                        response.Links = links;
                    });

                var actual = await _sut.GetDefault();

                actual.Should().BeOfType<OkObjectResult>();
                var expected = new ApiHomeResponse {
                    ApplicationInfo = _applicationInfo,
                    Links = links
                };
                actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
            }
        }
    }
}