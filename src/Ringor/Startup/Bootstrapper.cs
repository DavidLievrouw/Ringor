using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dalion.Ringor.Startup {
    internal static class Bootstrapper {
        private static readonly Assembly EntryAssembly = Assembly.GetEntryAssembly();

        public static int RunForResolvedEnvironment(string[] args) {
            var environment = new EnvironmentResolver().ResolveEnvironment(args);
            switch (environment) {
                case nameof(EnvironmentName.Development):
                    return RunDevelopment(args);
                default:
                    return RunProduction(args);
            }
        }

        public static int RunDevelopment(string[] args) {
            if (!bool.TryParse(Environment.GetEnvironmentVariable("DISABLE_HTTPSREDIRECTION"), out var disableHttpRedirection)) {
                disableHttpRedirection = false;
            }
            var bootstrapperSettings = new BootstrapperSettings {
                EnvironmentName = EnvironmentName.Development,
                EntryAssembly = EntryAssembly,
                UseDetailedErrors = true,
                DisableHttpsRedirection = disableHttpRedirection
            };
            var configuration = Configuration.BuildConfiguration(bootstrapperSettings, args);
            var webHost = BuildWebHost(configuration, bootstrapperSettings);
            webHost.Run();
            return 0;
        }

        private static int RunProduction(string[] args) {
            if (!bool.TryParse(Environment.GetEnvironmentVariable("DISABLE_HTTPSREDIRECTION"), out var disableHttpRedirection)) {
                disableHttpRedirection = false;
            }
            var bootstrapperSettings = new BootstrapperSettings {
                EnvironmentName = EnvironmentName.Production,
                EntryAssembly = EntryAssembly,
                UseDetailedErrors = false,
                DisableHttpsRedirection = disableHttpRedirection
            };
            var configuration = Configuration.BuildConfiguration(bootstrapperSettings, args);

            try {
                var webHost = BuildWebHost(configuration, bootstrapperSettings);
                webHost.Run();
                return 0;
            }
            catch (Exception) {
                return 1;
            }
        }

        internal static IWebHostBuilder CreateWebHostBuilder(IConfigurationRoot configuration, BootstrapperSettings settings) {
            return new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIIS()
                .UseIISIntegration()
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, settings.UseDetailedErrors.ToString())
                .UseConfiguration(configuration)
                .ConfigureAppConfiguration(Configuration.ReadSecretsFromAzureKeyVault)
                .ConfigureServices((context, services) => {
                    services.AddSingleton(context.Configuration);
                    services.AddSingleton(settings);
                })
                .UseStartup<WebHostStartup>()
                .ConfigureServices((context, services) => Composition.ConfigureServices(services, context.HostingEnvironment, context.Configuration, settings))
                .UseSerilog();
        }

        internal static IWebHost BuildWebHost(IConfigurationRoot configuration, BootstrapperSettings settings) {
            return CreateWebHostBuilder(configuration, settings).Build();
        }
    }
}