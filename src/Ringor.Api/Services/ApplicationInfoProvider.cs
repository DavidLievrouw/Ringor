using System;
using Dalion.Ringor.Api.Models;
using Microsoft.AspNetCore.Http;

namespace Dalion.Ringor.Api.Services {
    public class ApplicationInfoProvider : IApplicationInfoProvider {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationInfoProvider(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ApplicationInfo Provide() {
            return new ApplicationInfo {
                Message = "The Ringor application is running.",
                Version = GetType().Assembly.GetName().Version.ToString(fieldCount: 3),
                UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                    SiteUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value,
                    AppUrl = _httpContextAccessor.HttpContext.Request.PathBase
                }
            };
        }
    }
}