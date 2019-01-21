using System;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationInfoLinksCreatorFactory : IApplicationInfoLinksCreatorFactory {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public ApplicationInfoLinksCreatorFactory(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public ILinksCreator<ApplicationInfo> Create() {
            return new ApplicationInfoLinksCreator(_hyperlinkFactory);
        }
    }
}