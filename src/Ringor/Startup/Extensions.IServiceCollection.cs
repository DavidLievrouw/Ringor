using System;
using System.Collections.Generic;
using System.IO;
using Dalion.Ringor.Api.Services;
using Dalion.Ringor.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Dalion.Ringor.Startup {
    internal static partial class Extensions {
        public static TSettings ConfigureSettings<TSettings>(this IServiceCollection services, IConfigurationSection configSection, Func<TSettings> factory) where TSettings : class {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configSection == null) throw new ArgumentNullException(nameof(configSection));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            var settings = factory();
            configSection.Bind(settings);
            services.Configure<TSettings>(configSection);
            services.AddSingleton(settings);

            return settings;
        }

        public static TSettings ConfigureSettings<TSettings>(this IServiceCollection services, IConfigurationSection configSection) where TSettings : class, new() {
            return ConfigureSettings<TSettings>(services, configSection, () => new TSettings());
        }

        public static IServiceCollection AddApplicationInfo(this IServiceCollection services) {
            return services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IApplicationInfoProvider, ApplicationInfoProvider>();
        }

        public static IServiceCollection AddAzureAdAuthentication(this IServiceCollection services, AuthenticationSettings authSettings) {
            if (authSettings == null) throw new ArgumentNullException(nameof(authSettings));

            services
                .AddAuthorization()
                .AddAuthentication(o => { o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(o => {
                    o.Authority = (authSettings.SignInEndpoint?.AbsoluteUri?.TrimEnd('/') ?? string.Empty) + '/' + (authSettings.Tenant ?? string.Empty);
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
                        ValidAudiences = new List<string> {
                            authSettings.AppIdUri,
                            authSettings.ClientId
                        }
                    };
                    //o.Events = new JwtBearerEvents {
                    //    OnAuthenticationFailed = ctx => { return System.Threading.Tasks.Task.CompletedTask; },
                    //    OnTokenValidated = ctx => { return System.Threading.Tasks.Task.CompletedTask; }
                    //};
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, BootstrapperSettings bootstrapperSettings) {
            var appVersion = bootstrapperSettings.EntryAssembly.GetName().Version;
            return services.AddSwaggerGen(c => {
                c.SwaggerDoc(
                    "v" + appVersion.ToString(2),
                    new Info {
                        Title = "Ringor API",
                        Version = appVersion.ToString(2),
                        TermsOfService = "None"
                    });
                var pathToAddOn = bootstrapperSettings.EntryAssembly.Location;
                var pathToContentRoot = Path.GetDirectoryName(pathToAddOn);
                var assemblyName = Path.GetFileNameWithoutExtension(pathToAddOn);
                c.IncludeXmlComments(Path.Combine(pathToContentRoot, assemblyName + ".xml"));
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });
        }
    }
}