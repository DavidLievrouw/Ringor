using Dalion.Ringor.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Dalion.Ringor.Startup {
    internal static partial class Extensions {
        public static IWebHostBuilder ReadSecretsFromAzureKeyVault(this IWebHostBuilder builder) {
            return builder.ConfigureAppConfiguration((context, configuration) => {
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
            });
        }
    }
}