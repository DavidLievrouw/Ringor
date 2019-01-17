using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Startup {
    internal static class Bootstrapper {
        private static readonly Assembly EntryAssembly = Assembly.GetEntryAssembly();

        public static int RunForResolvedEnvironment(string[] args) {
            var environment = new EnvironmentResolver().ResolveEnvironment(args);
            switch (environment) {
                case nameof(EnvironmentName.Development):
                    return RunDevelopment(args);
                case nameof(EnvironmentName.Staging):
                    return RunStaging(args);
                default:
                    return RunProduction(args);
            }
        }

        public static int RunDevelopment(string[] args) {
            var bootstrapperSettings = new BootstrapperSettings {
                EnvironmentName = EnvironmentName.Development,
                EntryAssembly = EntryAssembly,
                UseDetailedErrors = true
            };
            var configuration = Configuration.BuildConfiguration(bootstrapperSettings, args);
            var webHost = BuildWebHost(configuration, bootstrapperSettings);
            webHost.Run();
            return 0;
        }

        public static int RunProduction(string[] args) {
            return RunStagingOrProduction(args, EnvironmentName.Production);
        }

        public static int RunStaging(string[] args) {
            return RunStagingOrProduction(args, EnvironmentName.Staging);
        }

        private static int RunStagingOrProduction(string[] args, string environmentName) {
            var bootstrapperSettings = new BootstrapperSettings {
                EnvironmentName = environmentName,
                EntryAssembly = EntryAssembly,
                UseDetailedErrors = environmentName != EnvironmentName.Production
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

        private static IWebHost BuildWebHost(IConfigurationRoot configuration, BootstrapperSettings settings) {
            return new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIIS()
                .UseIISIntegration()
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, settings.UseDetailedErrors.ToString())
                .UseConfiguration(configuration)
                .ConfigureServices((context, s) => {
                    s.AddSingleton(configuration);
                    s.AddSingleton(settings);
                })
                .UseStartup<WebHostStartup>()
                .ConfigureServices((ctx, s) => Composition.ConfigureServices(s, ctx, configuration, settings))
                .Build();
        }
    }
}