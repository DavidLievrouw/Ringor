using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApiHomeResponseLinksCreatorFactoryTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly ApiHomeResponseLinksCreatorFactory _sut;

        public ApiHomeResponseLinksCreatorFactoryTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new ApiHomeResponseLinksCreatorFactory(_hyperlinkFactory);
        }

        public class Create : ApiHomeResponseLinksCreatorFactoryTests {
            [Fact]
            public void ReturnsInstanceOfExpectedType() {
                var actual = _sut.Create();
                actual.Should().NotBeNull().And.BeOfType<ApiHomeResponseLinksCreator>();
            }
        }
    }
}