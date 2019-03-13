using System;
using System.Net.Http;
using Dalion.Ringor.Utils;

namespace Dalion.Ringor.Api.Models.Links {
    public class HyperlinkFactory : IHyperlinkFactory {
        private readonly IApplicationUriResolver _applicationUriResolver;

        public HyperlinkFactory(IApplicationUriResolver applicationUriResolver) {
            _applicationUriResolver = applicationUriResolver ?? throw new ArgumentNullException(nameof(applicationUriResolver));
        }

        public Hyperlink<TRel> Create<TRel>(HttpMethod method, string href, TRel rel)
            where TRel : struct, IConvertible {
            var applicationUri = _applicationUriResolver.Resolve();
            var relativePath = $"{href.TrimStart('/')}".TrimEnd('/');
            return new Hyperlink<TRel>(method, applicationUri.WithRelativePath(relativePath).ToString(), rel);
        }
    }
}