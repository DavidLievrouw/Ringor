using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class DelegatedPermissionRequirement : AuthorizationHandler<DelegatedPermissionRequirement>, IAuthorizationRequirement {
        public string[] Permissions { get; set; }
        public Action<AuthorizationHandlerContext, DelegatedPermissionRequirement> RequirementNotFulfilledCallback { get; set; }
        public Action<AuthorizationHandlerContext, DelegatedPermissionRequirement> RequirementFulfilledCallback { get; set; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DelegatedPermissionRequirement requirement) {
            if (context.User.Identity.IsAuthenticated && (requirement.Permissions?.Any(p => context.User.HasClaim(Constants.ClaimTypes.Scope, p)) ?? true)) {
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