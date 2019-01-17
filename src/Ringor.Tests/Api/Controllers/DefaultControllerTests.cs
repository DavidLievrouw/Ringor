using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Dalion.Ringor.Api.Controllers {
    public class DefaultControllerTests {
        private readonly IApplicationInfoProvider _applicationInfoProvider;
        private readonly DefaultController _sut;

        public DefaultControllerTests() {
            FakeFactory.Create(out _applicationInfoProvider);
            _sut = new DefaultController(_applicationInfoProvider);
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
            public void ReturnsOkWithApplicationInfo() {
                var actual = _sut.GetDefault();
                actual.Should().BeOfType<OkObjectResult>();
                actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(_applicationInfo);
            }
        }
    }
}