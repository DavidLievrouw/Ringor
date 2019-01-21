using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class UserInfoResponseLinksCreatorFactoryTests {
        private readonly IHyperlinkFactory _hyperlinkFactory;
        private readonly UserInfoResponseLinksCreatorFactory _sut;

        public UserInfoResponseLinksCreatorFactoryTests() {
            FakeFactory.Create(out _hyperlinkFactory);
            _sut = new UserInfoResponseLinksCreatorFactory(_hyperlinkFactory);
        }

        public class Create : UserInfoResponseLinksCreatorFactoryTests {
            [Fact]
            public void ReturnsInstanceOfExpectedType() {
                var actual = _sut.Create();
                actual.Should().NotBeNull().And.BeOfType<UserInfoResponseLinksCreator>();
            }
        }
    }
}