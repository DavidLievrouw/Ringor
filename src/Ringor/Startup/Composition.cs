using Dalion.Ringor.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Startup {
    internal static class Composition {
        public static void ConfigureServices(IServiceCollection services, WebHostBuilderContext context, IConfiguration configuration) {
            services.Configure<RingSettings>(configuration.GetSection("RingSettings"));
            services.Configure<SftpSettings>(configuration.GetSection("SftpSettings"));
        }
    }
}