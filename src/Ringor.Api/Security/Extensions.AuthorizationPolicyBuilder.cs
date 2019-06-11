using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public static class AuthorizationPolicyBuilderExtensions {
        public static AuthorizationPolicyBuilder RequireDelegatedPermissions(
            this AuthorizationPolicyBuilder builder,
            params string[] delegated) {
            builder.Requirements.Add(new DelegatedPermissionRequirement {Permissions = delegated ?? Array.Empty<string>()});
            return builder;
        }

        public static AuthorizationPolicyBuilder RequireApplicationPermissions(
            this AuthorizationPolicyBuilder builder,
            params string[] application) {
            builder.Requirements.Add(new ApplicationPermissionRequirement {Permissions = application ?? Array.Empty<string>()});
            return builder;
        }

        public static AuthorizationPolicyBuilder Any(this AuthorizationPolicyBuilder builder, Action<AuthorizationPolicyBuilder> configAction) {
            var subBuilder = new AuthorizationPolicyBuilder();
            configAction(subBuilder);
            builder.AddRequirements(new AnyRequirement(subBuilder.Requirements.OfType<IAuthorizationHandler>().ToArray()));
            return builder;
        }
    }
}