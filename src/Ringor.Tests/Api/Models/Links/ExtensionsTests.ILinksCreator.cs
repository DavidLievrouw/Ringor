using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dalion.Ringor.Utils;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public partial class ExtensionsTests {
        public class CreateLinksFor : ExtensionsTests {
            private readonly ILinksCreator<ApiHomeResponse> _linksCreator;
            private readonly ApiHomeResponse[] _appInfos;

            public CreateLinksFor() {
                _linksCreator = A.Fake<ILinksCreator<ApiHomeResponse>>();
                _appInfos = new[] {
                    new ApiHomeResponse {ApplicationInfo = new ApplicationInfo {Product = Guid.NewGuid().ToString()}},
                    new ApiHomeResponse {ApplicationInfo = new ApplicationInfo {Product = Guid.NewGuid().ToString()}},
                    new ApiHomeResponse {ApplicationInfo = new ApplicationInfo {Product = Guid.NewGuid().ToString()}}
                };
            }

            [Fact]
            public void GivenNullModels_Throws() {
                Func<Task> act = () => _linksCreator.CreateLinksFor((IEnumerable<ApiHomeResponse>) null);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenEmptyModels_DoesNotThrow_DoesNotDoAnything() {
                Func<Task> act = () => _linksCreator.CreateLinksFor(Enumerable.Empty<ApiHomeResponse>());
                act.Should().NotThrow();
                A.CallTo(() => _linksCreator.CreateLinksFor(A<ApiHomeResponse>._)).MustNotHaveHappened();
            }

            [Fact]
            public async Task CallsLinksCreatorForAllModels() {
                var someLinks = new[] {
                    new Hyperlink<ApiHomeResponseHyperlinkType>(HttpMethod.Put, "ABC", ApiHomeResponseHyperlinkType.GetUserInfo)
                };
                A.CallTo(() => _linksCreator.CreateLinksFor(A<ApiHomeResponse>._))
                    .Invokes(call => call.GetArgument<ApiHomeResponse>(0).Links = someLinks);

                await _linksCreator.CreateLinksFor(_appInfos);

                _appInfos.ForEach(s => s.Links.Should().BeEquivalentTo<Hyperlink<ApiHomeResponseHyperlinkType>>(someLinks));
            }
        }
    }
}