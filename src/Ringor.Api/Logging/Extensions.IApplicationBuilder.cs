using System;
using Microsoft.AspNetCore.Builder;

namespace Dalion.Ringor.Api.Logging {
    public static partial class Extensions {
        public static IApplicationBuilder UseUnhandledExceptionLogging(this IApplicationBuilder app) {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<UnhandledExceptionLoggingMiddleware>();
        }
    }
}