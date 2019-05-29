using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class PermissionRequirement : IAuthorizationRequirement {
        public string[] DelegatedPermissions { get; set; }
        public string[] ApplicationPermissions { get; set; }

        public override string ToString() {
            var delegatedPermissions = string.Join(", ", DelegatedPermissions ?? Enumerable.Empty<string>());
            var applicationPermissions = string.Join(", ", ApplicationPermissions ?? Enumerable.Empty<string>());
            return $"Delegated permissions: [{delegatedPermissions}], Application permissions: [{applicationPermissions}]";
        }
    }
}