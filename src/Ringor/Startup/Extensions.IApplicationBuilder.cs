using Microsoft.AspNetCore.Builder;

namespace Dalion.Ringor.Startup {
    internal static partial class Extensions {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app) {
            return app
                .UseSwagger(c => { c.PreSerializeFilters.Add((swaggerDoc, httpRequest) => swaggerDoc.Host = httpRequest.Host.Value); })
                .UseSwaggerUI(c => {
                    var appVersion = typeof(Program).Assembly.GetName().Version;
                    c.SwaggerEndpoint("/swagger/v" + appVersion.ToString(2) + "/swagger.json", "Ringor API v" + appVersion.ToString(2));
                });
        }
    }
}