using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationInfoLinksCreatorFactoryTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly ApplicationInfoLinksCreatorFactory _sut;

        public ApplicationInfoLinksCreatorFactoryTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new ApplicationInfoLinksCreatorFactory(_hyperlinkFactory);
        }

        public class Create : ApplicationInfoLinksCreatorFactoryTests {
            [Fact]
            public void ReturnsInstanceOfExpectedType() {
                var actual = _sut.Create();
                actual.Should().NotBeNull().And.BeOfType<ApplicationInfoLinksCreator>();
            }
        }
    }
}