using System.IO;
using Microsoft.Extensions.Configuration;

namespace Dalion.Ringor.Startup {
    internal static class Configuration {
        public static IConfigurationRoot BuildConfiguration(BootstrapperSettings settings, string[] args) {
            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(settings.EntryAssembly.Location))
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{settings.EnvironmentName}.json", true, true)
                .AddUserSecrets<Program>()
                .Build();
        }
    }
}