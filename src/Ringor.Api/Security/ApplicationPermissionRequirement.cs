using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class ApplicationPermissionRequirement : IAuthorizationRequirement {
        public string[] Permissions { get; set; }

        public override string ToString() {
            var permissions = string.Join(", ", Permissions ?? Enumerable.Empty<string>());
            return $"Application permissions: [{permissions}]";
        }
    }
}