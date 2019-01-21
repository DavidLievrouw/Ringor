using System;
using Dalion.Ringor.Api.Services;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationUriResolver : IApplicationUriResolver {
        private readonly IApplicationInfoProvider _applicationInfoProvider;

        public ApplicationUriResolver(IApplicationInfoProvider applicationInfoProvider) {
            _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
        }

        public Uri Resolve() {
            string Normalize(string url) {
                return string.IsNullOrEmpty(url) ? string.Empty
                    : url.StartsWith("/") ? url.ToLowerInvariant().Substring(1) : url.ToLowerInvariant();
            }

            var applicationInfo = _applicationInfoProvider.Provide();
            var sitePath = applicationInfo.UrlInfo.SiteUrl;
            var applicationPath = Normalize(applicationInfo.UrlInfo.AppUrl);

            var baseUrl = $"{sitePath.TrimEnd('/')}/{applicationPath.TrimEnd('/')}";
            baseUrl = $"{baseUrl.TrimEnd('/')}/";

            return new Uri(baseUrl, UriKind.Absolute);
        }
    }
}