using System.IO;
using Dalion.Ringor.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Dalion.Ringor.Startup {
    internal static class Configuration {
        public static IConfigurationRoot BuildConfiguration(BootstrapperSettings settings, string[] args) {
            return new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(settings.EntryAssembly.Location))
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{settings.EnvironmentName}.json", true, true)
                .AddUserSecrets<Program>()
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();
        }

        public static void ReadSecretsFromAzureKeyVault(WebHostBuilderContext context, IConfigurationBuilder configuration) {
            if (!context.HostingEnvironment.IsDevelopment()) {
                var secretsSettings = new SecretsSettings();
                context.Configuration.GetSection("Secrets").Bind(secretsSettings);
                configuration.AddAzureKeyVault(
                    secretsSettings.KeyVaultUri.AbsoluteUri,
                    new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(
                            new AzureServiceTokenProvider().KeyVaultTokenCallback)),
                    new DefaultKeyVaultSecretManager());
            }
        }
    }
}