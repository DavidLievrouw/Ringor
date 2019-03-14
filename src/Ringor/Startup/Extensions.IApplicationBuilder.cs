using System;
using System.Collections.Generic;
using Dalion.Ringor.Configuration;
using Dalion.Ringor.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Dalion.Ringor.Startup {
    internal static partial class Extensions {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, AuthenticationSettings authenticationSettings, IApiVersionDescriptionProvider provider) {
            return app
                .UseSwagger(options => { options.PreSerializeFilters.Add((swaggerDoc, httpRequest) => swaggerDoc.Host = httpRequest.Host.Value); })
                .UseSwaggerUI(options => {
                    foreach (var description in provider.ApiVersionDescriptions) {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant()
                        );
                    }
                    options.OAuthClientId(authenticationSettings.Swagger.ClientId);
                    options.OAuthAppName("Ringor Swagger UI");
                    options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> {
                        {"resource", authenticationSettings.AppIdUri}
                    });
                });
        }

        public static IApplicationBuilder UseUnhandledExceptionLogging(this IApplicationBuilder app) {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<UnhandledExceptionLoggingMiddleware>();
        }
    }
}