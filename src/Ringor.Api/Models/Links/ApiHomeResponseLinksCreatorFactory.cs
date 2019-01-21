using System;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApiHomeResponseLinksCreatorFactory : IApiHomeResponseLinksCreatorFactory {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public ApiHomeResponseLinksCreatorFactory(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public ILinksCreator<ApiHomeResponse> Create() {
            return new ApiHomeResponseLinksCreator(_hyperlinkFactory);
        }
    }
}