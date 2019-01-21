using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public partial class ExtensionsTests {
        public class CreateLinksFor : ExtensionsTests {
            private readonly ILinksCreator<ApplicationInfo> _linksCreator;
            private readonly ApplicationInfo[] _appInfos;

            public CreateLinksFor() {
                _linksCreator = A.Fake<ILinksCreator<ApplicationInfo>>();
                _appInfos = new[] {
                    new ApplicationInfo {Message = Guid.NewGuid().ToString()},
                    new ApplicationInfo {Message = Guid.NewGuid().ToString()},
                    new ApplicationInfo {Message = Guid.NewGuid().ToString()}
                };
            }

            [Fact]
            public void GivenNullModels_Throws() {
                Func<Task> act = () => _linksCreator.CreateLinksFor((IEnumerable<ApplicationInfo>) null);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenEmptyModels_DoesNotThrow_DoesNotDoAnything() {
                Func<Task> act = () => _linksCreator.CreateLinksFor(Enumerable.Empty<ApplicationInfo>());
                act.Should().NotThrow();
                A.CallTo(() => _linksCreator.CreateLinksFor(A<ApplicationInfo>._)).MustNotHaveHappened();
            }

            [Fact]
            public async Task CallsLinksCreatorForAllModels() {
                var someLinks = new[] {
                    new Hyperlink<ApplicationInfoHyperlinkType>(HttpMethod.Put, "ABC", ApplicationInfoHyperlinkType.GetUserInfo)
                };
                A.CallTo(() => _linksCreator.CreateLinksFor(A<ApplicationInfo>._))
                    .Invokes(call => call.GetArgument<ApplicationInfo>(0).Links = someLinks);

                await _linksCreator.CreateLinksFor(_appInfos);

                _appInfos.ForEach(s => s.Links.Should().BeEquivalentTo(someLinks));
            }
        }
    }
}