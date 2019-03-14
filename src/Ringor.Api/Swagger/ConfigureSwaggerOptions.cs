using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dalion.Ringor.Api.Swagger {
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions> {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options) {
            foreach (var description in _provider.ApiVersionDescriptions) {
                var groupInfo = new Info {
                    Title = $"Ringor API v{description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Contact = new Contact {Name = "David Lievrouw", Email = "info@dalion.eu"},
                    License = new License {Name = "MIT", Url = "https://opensource.org/licenses/MIT"},
                    Description = "The Ringor API."
                };

                if (description.IsDeprecated) {
                    groupInfo.Description += " This API version has been deprecated.";
                }

                options.SwaggerDoc(description.GroupName, groupInfo);
            }
        }
    }
}