using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class ApplicationPermissionRequirement : AuthorizationHandler<ApplicationPermissionRequirement>, IAuthorizationRequirement {
        public string[] Permissions { get; set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationPermissionRequirement requirement) {
            if (requirement.Permissions.Any(p => context.User.HasClaim(Constants.ClaimTypes.Role, p))) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}