using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Startup {
    internal static class Composition {
        public static void ConfigureServices(IServiceCollection services, WebHostBuilderContext context, IConfiguration configuration) {
            // Add additional registrations here
        }
    }
}