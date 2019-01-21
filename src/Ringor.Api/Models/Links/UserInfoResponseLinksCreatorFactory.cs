using System;

namespace Dalion.Ringor.Api.Models.Links {
    public class UserInfoResponseLinksCreatorFactory : IUserInfoResponseLinksCreatorFactory {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public UserInfoResponseLinksCreatorFactory(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public ILinksCreator<UserInfoResponse> Create() {
            return new UserInfoResponseLinksCreator(_hyperlinkFactory);
        }
    }
}