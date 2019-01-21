using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ClaimLinksCreatorFactoryTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly ClaimLinksCreatorFactory _sut;

        public ClaimLinksCreatorFactoryTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new ClaimLinksCreatorFactory(_hyperlinkFactory);
        }

        public class Create : ClaimLinksCreatorFactoryTests {
            [Fact]
            public void ReturnsInstanceOfExpectedType() {
                var actual = _sut.Create();
                actual.Should().NotBeNull().And.BeOfType<ClaimLinksCreator>();
            }
        }
    }
}