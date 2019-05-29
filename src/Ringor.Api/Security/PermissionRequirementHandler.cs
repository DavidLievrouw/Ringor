using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Dalion.Ringor.Api.Security {
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement> {
        private readonly ILogger<PermissionRequirementHandler> _logger;

        public PermissionRequirementHandler(ILogger<PermissionRequirementHandler> logger) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement) {
            var isSuccess = false;

            if (requirement.DelegatedPermissions.Any(p => {
                var hasClaim = context.User.HasClaim(Constants.ClaimTypes.Scope, p);
                _logger.LogDebug(hasClaim
                    ? $"The current user has delegated permission '{p}'."
                    : $"The current user does not have delegated permission '{p}'.");
                return hasClaim;
            })) {
                isSuccess = Succeed(context, requirement);
            }
            else if (requirement.ApplicationPermissions.Any(p => {
                var hasClaim = context.User.HasClaim(Constants.ClaimTypes.Role, p);
                _logger.LogDebug(hasClaim
                    ? $"The current user has application permission '{p}'."
                    : $"The current user does not have application permission '{p}'.");
                return hasClaim;
            })) {
                isSuccess = Succeed(context, requirement);
            }

            if (!isSuccess) {
                _logger.LogWarning($"Authorization failed for requirement {requirement}. The current principal does not have any claim satisfying the requirement.");
            }

            return Task.CompletedTask;
        }

        private bool Succeed(AuthorizationHandlerContext context, PermissionRequirement requirement) {
            context.Succeed(requirement);
            _logger.LogDebug("Authorization succeeded.");
            return true;
        }
    }
}