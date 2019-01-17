using Dalion.Ringor.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Startup {
    internal static class Composition {
        public static void ConfigureServices(IServiceCollection services, WebHostBuilderContext context, IConfiguration configuration, BootstrapperSettings bootstrapperSettings) {
            // Configuration
            var ringSettings = services.ConfigureSettings<RingSettings>(configuration.GetSection("RingSettings"));
            var sftpSettings = services.ConfigureSettings<SftpSettings>(configuration.GetSection("SftpSettings"));
            var authSettings = services.ConfigureSettings<AuthenticationSettings>(configuration.GetSection("Authentication"));

            // Features and services
            services
                .AddAzureAdAuthentication(authSettings)
                .AddSwagger(bootstrapperSettings, authSettings)
                .AddApplicationInfo();
        }
    }
}