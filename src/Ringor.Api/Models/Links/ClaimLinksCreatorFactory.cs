using System;

namespace Dalion.Ringor.Api.Models.Links {
    public class ClaimLinksCreatorFactory : IClaimLinksCreatorFactory {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public ClaimLinksCreatorFactory(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public ILinksCreator<Claim> Create() {
            return new ClaimLinksCreator(_hyperlinkFactory);
        }
    }
}