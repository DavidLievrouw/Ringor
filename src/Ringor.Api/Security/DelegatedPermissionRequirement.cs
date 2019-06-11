using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class DelegatedPermissionRequirement : AuthorizationHandler<DelegatedPermissionRequirement>, IAuthorizationRequirement {
        public string[] Permissions { get; set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DelegatedPermissionRequirement requirement) {
            if (requirement.Permissions.Any(p => context.User.HasClaim(Constants.ClaimTypes.Scope, p))) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}