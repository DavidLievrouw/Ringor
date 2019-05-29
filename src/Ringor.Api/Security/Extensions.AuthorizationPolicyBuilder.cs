using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public static class AuthorizationPolicyBuilderExtensions {
        public static void RequirePermissions(
            this AuthorizationPolicyBuilder builder,
            string[] delegated,
            string[] application = null) {
            builder.RequireAuthenticatedUser();
            builder.Requirements.Add(new PermissionRequirement {
                DelegatedPermissions = delegated,
                ApplicationPermissions = application ?? new string[0]
            });
        }
    }
}