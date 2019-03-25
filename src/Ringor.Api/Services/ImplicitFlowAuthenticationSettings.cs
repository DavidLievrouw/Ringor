using System;

namespace Dalion.Ringor.Api.Services {
    public class ImplicitFlowAuthenticationSettings {
        public Uri Authority { get; set; }
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string[] Scopes { get; set; }
    }
}