using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Dalion.Ringor.Api.Security {
    public class ApplicationPermissionRequirementHandler : AuthorizationHandler<ApplicationPermissionRequirement> {
        private readonly ILogger<ApplicationPermissionRequirementHandler> _logger;

        public ApplicationPermissionRequirementHandler(ILogger<ApplicationPermissionRequirementHandler> logger) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationPermissionRequirement requirement) {
            var isSuccess = false;

            if (requirement.Permissions.Any(p => {
                var hasClaim = context.User.HasClaim(Constants.ClaimTypes.Role, p);
                _logger.LogDebug(hasClaim
                    ? $"The current user has application permission '{p}'."
                    : $"The current user does not have application permission '{p}'.");
                return hasClaim;
            })) {
                isSuccess = Succeed(context, requirement);
            }

            if (!isSuccess) {
                _logger.LogWarning($"Authorization failed for requirement {requirement}. The current principal does not have any role claim satisfying the requirement.");
            }

            return Task.CompletedTask;
        }

        private bool Succeed(AuthorizationHandlerContext context, IAuthorizationRequirement requirement) {
            context.Succeed(requirement);
            _logger.LogDebug("Authorization succeeded.");
            return true;
        }
    }
}