using System;

namespace Dalion.Ringor.Configuration {
    public class AuthenticationSettings {
        public Uri SignInEndpoint { get; set; }
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string AppIdUri { get; set; }
        public string[] Scopes { get; set; }
        public SwaggerAuthenticationSettings Swagger { get; set; }

        public class SwaggerAuthenticationSettings {
            public string ClientId { get; set; }
        }
    }
}