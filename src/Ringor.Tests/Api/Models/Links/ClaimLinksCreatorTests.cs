using System;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ClaimLinksCreatorTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly ClaimLinksCreator _sut;

        public ClaimLinksCreatorTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new ClaimLinksCreator(_hyperlinkFactory);
        }

        public class CreateLinksFor : ClaimLinksCreatorTests {
            private readonly Claim _claim;

            public CreateLinksFor() {
                _claim = new Claim {Type = "c1", Value = "v1"};
                A.CallTo(() => _hyperlinkFactory.Create(A<HttpMethod>._, A<string>._, A<ClaimHyperlinkType>._))
                    .ReturnsLazily(call => {
                        var method = call.GetArgument<HttpMethod>(0);
                        var relativeUrl = call.GetArgument<string>(1);
                        var rel = call.GetArgument<ClaimHyperlinkType>(2);
                        return new Hyperlink<ClaimHyperlinkType>(method, $"https://dalion.eu/testing{relativeUrl}", rel);
                    });
            }

            [Fact]
            public void GivenNullModel_DoesNotThrow() {
                Action act = () => _sut.CreateLinksFor(null);
                act.Should().NotThrow();
            }

            [Fact]
            public async Task AddsExpectedLinksToModel() {
                await _sut.CreateLinksFor(_claim);
                var expected = new Claim {
                    Type = "c1",
                    Value = "v1",
                    Links = new[] {
                        new Hyperlink<ClaimHyperlinkType>(HttpMethod.Get, "https://dalion.eu/testing/api/userinfo/c1", ClaimHyperlinkType.EnumerateAllClaimsOfThisType),
                        new Hyperlink<ClaimHyperlinkType>(HttpMethod.Get, "https://dalion.eu/testing/api/userinfo", ClaimHyperlinkType.GetUserInfo)
                    }
                };
                var differences = _claim.CompareTo(expected);
                differences.AreEqual.Should().BeTrue(because: differences.DifferencesString);
            }
        }
    }
}