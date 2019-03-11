using System;
using Dalion.Ringor.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Dalion.Ringor {
    public class CustomWebApplicationFactory : WebApplicationFactory<WebHostStartup> {
        protected override IWebHostBuilder CreateWebHostBuilder() {
            var bootstrapperSettings = new BootstrapperSettings {
                EnvironmentName = EnvironmentName.Staging,
                EntryAssembly = typeof(Bootstrapper).Assembly,
                UseDetailedErrors = true
            };
            var configuration = Startup.Configuration.BuildConfiguration(bootstrapperSettings, Array.Empty<string>());
            return Bootstrapper.CreateWebHostBuilder(configuration, bootstrapperSettings);
        }
    }
}