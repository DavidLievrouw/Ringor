using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dalion.Ringor.Api.Swagger {
    public class AuthorizeCheckOperationFilter : IOperationFilter {
        public void Apply(Operation operation, OperationFilterContext context) {
            var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .Any();

            if (hasAuthorizeAttribute) {
                operation.Responses.Add("401", new Response {Description = "When there is no authenticated user or the authentication failed (Unauthorized)."});
                operation.Responses.Add("403", new Response {Description = "When the authenticated user is not allowed to perform this request (Forbidden)"});

                operation.Security = new List<IDictionary<string, IEnumerable<string>>> {
                    new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] {"ringor_api"}}}
                };
            }
        }
    }
}