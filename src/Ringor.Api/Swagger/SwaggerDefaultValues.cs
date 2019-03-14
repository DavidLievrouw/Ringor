using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dalion.Ringor.Api.Swagger {
    public class SwaggerDefaultValues : IOperationFilter {
        public void Apply(Operation operation, OperationFilterContext context) {
            var apiDescription = context.ApiDescription;
            var apiVersion = apiDescription.GetApiVersion();
            var model = apiDescription.ActionDescriptor.GetApiVersionModel(ApiVersionMapping.Implicit | ApiVersionMapping.Explicit);

            operation.Deprecated = model.DeprecatedApiVersions.Contains(apiVersion);

            if (operation.Parameters == null) {
                return;
            }

            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>()) {
                var description = apiDescription.ParameterDescriptions
                    .First(p => p.Name == parameter.Name);

                if (parameter.Description == null) {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Default == null) {
                    parameter.Default = description.DefaultValue;
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}