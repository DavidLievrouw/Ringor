using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class DelegatedPermissionRequirement : IAuthorizationRequirement {
        public string[] Permissions { get; set; }

        public override string ToString() {
            var permissions = string.Join(", ", Permissions ?? Enumerable.Empty<string>());
            return $"Delegated permissions: [{permissions}]";
        }
    }
}