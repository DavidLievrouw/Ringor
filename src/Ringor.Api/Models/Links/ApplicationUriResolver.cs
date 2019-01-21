using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationUriResolver : IApplicationUriResolver {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationUriResolver(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Uri Resolve() {
            string Normalize(string url) {
                return string.IsNullOrEmpty(url) ? "/"
                    : url.StartsWith("/") ? url.ToLowerInvariant() : "/" + url.ToLowerInvariant();
            }

            var requestUri = new Uri(_httpContextAccessor.HttpContext.Request.GetDisplayUrl());
            var hostUrl = requestUri.GetLeftPart(UriPartial.Authority);
            var applicationPath = Normalize(_httpContextAccessor.HttpContext.Request.PathBase);

            var baseUrl = $"{hostUrl}{applicationPath.TrimEnd('/')}/";
            return new Uri(baseUrl, UriKind.Absolute);
        }
    }
}