using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class ApplicationPermissionRequirement : AuthorizationHandler<ApplicationPermissionRequirement>, IAuthorizationRequirement {
        public string[] Permissions { get; set; }
        public Action<AuthorizationHandlerContext, ApplicationPermissionRequirement> RequirementNotFulfilledCallback { get; set; }
        public Action<AuthorizationHandlerContext, ApplicationPermissionRequirement> RequirementFulfilledCallback { get; set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationPermissionRequirement requirement) {
            if (context.User.Identity.IsAuthenticated && (requirement.Permissions?.Any(p => context.User.HasClaim(Constants.ClaimTypes.Role, p)) ?? true)) {
                context.Succeed(requirement);
                RequirementFulfilledCallback?.Invoke(context, requirement);
            }
            else {
                RequirementNotFulfilledCallback?.Invoke(context, requirement);
            }

            return Task.CompletedTask;
        }
    }
}