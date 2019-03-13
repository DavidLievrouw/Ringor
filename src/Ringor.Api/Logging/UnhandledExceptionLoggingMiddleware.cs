using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Dalion.Ringor.Api.Logging {
    public class UnhandledExceptionLoggingMiddleware {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public UnhandledExceptionLoggingMiddleware(RequestDelegate next, ILogger logger) {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            }
            catch (Exception ex) {
                _logger.Fatal(ex, "An unhandled exception was thrown by the application.");
                throw;
            }
        }
    }
}