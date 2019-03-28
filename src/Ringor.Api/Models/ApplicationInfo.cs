using System;

namespace Dalion.Ringor.Api.Models {
    public class ApplicationInfo {
        public string Company { get; set; }
        public string Product { get; set; }
        public string Email { get; set; }
        public string Version { get; set; }
        public ApplicationUrlInfo UrlInfo { get; set; }
        public string Environment { get; set; }
        public ApplicationAuthenticationInfo AuthenticationInfo { get; set; }

        public class ApplicationUrlInfo {
            public string SiteUrl { get; set; }
            public string AppUrl { get; set; }
        }
        
        public class ApplicationAuthenticationInfo {
            public Uri Authority { get; set; }
            public string Tenant { get; set; }
            public string ClientId { get; set; }
            public string[] Scopes { get; set; }
        }
    }
}