using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Dalion.Ringor.Api.Security {
    public class AnyRequirement : AuthorizationHandler<AnyRequirement>, IAuthorizationRequirement {
        private readonly IEnumerable<IAuthorizationHandler> _handlers;

        public AnyRequirement(params IAuthorizationHandler[] handlers) {
            _handlers = handlers;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AnyRequirement requirement) {
            foreach (var handler in _handlers) {
                var subContext = new AuthorizationHandlerContext(new[] {(IAuthorizationRequirement) handler}, context.User, context.Resource);
                await handler.HandleAsync(subContext);
                if (subContext.HasSucceeded) {
                    context.Succeed(this);
                    break;
                }
            }
        }
    }
}