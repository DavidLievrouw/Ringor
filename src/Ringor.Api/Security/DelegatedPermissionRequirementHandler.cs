using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Dalion.Ringor.Api.Security {
    public class DelegatedPermissionRequirementHandler : AuthorizationHandler<DelegatedPermissionRequirement> {
        private readonly ILogger<DelegatedPermissionRequirementHandler> _logger;

        public DelegatedPermissionRequirementHandler(ILogger<DelegatedPermissionRequirementHandler> logger) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DelegatedPermissionRequirement requirement) {
            var isSuccess = false;

            if (requirement.Permissions.Any(p => {
                var hasClaim = context.User.HasClaim(Constants.ClaimTypes.Scope, p);
                _logger.LogDebug(hasClaim
                    ? $"The current user has delegated permission '{p}'."
                    : $"The current user does not have delegated permission '{p}'.");
                return hasClaim;
            })) {
                isSuccess = Succeed(context, requirement);
            }

            if (!isSuccess) {
                _logger.LogWarning($"Authorization failed for requirement {requirement}. The current principal does not have any scope claim satisfying the requirement.");
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