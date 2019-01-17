using System.Collections.Generic;
using Dalion.Ringor.Configuration;
using Microsoft.AspNetCore.Builder;

namespace Dalion.Ringor.Startup {
    internal static partial class Extensions {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, AuthenticationSettings authenticationSettings) {
            return app
                .UseSwagger(c => {
                    c.PreSerializeFilters.Add((swaggerDoc, httpRequest) => swaggerDoc.Host = httpRequest.Host.Value);
                })
                .UseSwaggerUI(c => {
                    var appVersion = typeof(Program).Assembly.GetName().Version;
                    c.SwaggerEndpoint("/swagger/v" + appVersion.ToString(2) + "/swagger.json", "Ringor API v" + appVersion.ToString(2));
                    c.OAuthClientId(authenticationSettings.Swagger.ClientId);
                    c.OAuthAppName("Ringor Swagger UI");
                    c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> {
                        {"resource", authenticationSettings.AppIdUri}
                    });
                });
        }
    }
}