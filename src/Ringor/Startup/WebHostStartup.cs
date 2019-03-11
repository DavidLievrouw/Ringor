﻿using System;
using Dalion.Ringor.Api.Controllers;
using Dalion.Ringor.Api.Serialization;
using Dalion.Ringor.Configuration;
using Dalion.Ringor.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dalion.Ringor.Startup {
    public class WebHostStartup : IStartup {
        private readonly IHostingEnvironment _environment;

        public WebHostStartup(IHostingEnvironment environment) {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public IServiceProvider ConfigureServices(IServiceCollection services) {
            // General
            services.TryAddSingleton(_environment);

            // Mvc
            services
                .AddRouting(options => options.LowercaseUrls = true)
                .AddMvc()
                .AddJsonOptions(config => PreConfiguredJsonSerializerSettings.Apply(config.SerializerSettings))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApplicationPart(typeof(DefaultController).Assembly)
                .AddControllersAsServices();

            // Compose
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app) {
            app = _environment.IsDevelopmentOrDebug()
                ? app.UseDeveloperExceptionPage()
                : app.UseExceptionHandler("/error");

            app
                .UseHttpsRedirection()
                .UseStatusCodePagesWithReExecute("/error/{0}")
                .UseAuthentication() // Very important that this is called before anything that will require authentication
                .UseMvc()
                .UseStaticFiles()
                .UseSwagger(app.ApplicationServices.GetService<AuthenticationSettings>());
        }
    }
}