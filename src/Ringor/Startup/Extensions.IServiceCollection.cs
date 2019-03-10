using System;
using System.Collections.Generic;
using System.IO;
using Dalion.Ringor.Api.Controllers;
using Dalion.Ringor.Api.Models.Links;
using Dalion.Ringor.Api.Security;
using Dalion.Ringor.Api.Serialization;
using Dalion.Ringor.Api.Services;
using Dalion.Ringor.Configuration;
using Dalion.Ringor.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
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
            return ConfigureSettings(services, configSection, () => new TSettings());
        }

        public static IServiceCollection AddApplicationInfo(this IServiceCollection services) {
            return services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IApplicationInfoProvider>(prov => new ApplicationInfoProvider(
                    prov.GetRequiredService<IHttpContextAccessor>(),
                    prov.GetRequiredService<BootstrapperSettings>().EntryAssembly,
                    prov.GetRequiredService<BootstrapperSettings>().EnvironmentName));
        }

        public static IServiceCollection AddAzureAdAuthentication(this IServiceCollection services, AuthenticationSettings authSettings) {
            if (authSettings == null) throw new ArgumentNullException(nameof(authSettings));

            services
                .AddAuthorization(options => {
                    options.AddPolicy(AuthorizationPolicies.RequireApiAccess, policy => policy.RequireClaim(ClaimTypes.Scope, "Api.FullAccess"));
                    options.DefaultPolicy = options.GetPolicy(AuthorizationPolicies.RequireApiAccess);
                })
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

        public static IServiceCollection AddSwagger(this IServiceCollection services, BootstrapperSettings bootstrapperSettings, AuthenticationSettings authSettings) {
            var appVersion = bootstrapperSettings.EntryAssembly.GetName().Version;
            return services.AddSwaggerGen(c => {
                c.SwaggerDoc(
                    "v" + appVersion.ToString(2),
                    new Info {
                        Title = "Ringor API",
                        Version = appVersion.ToString(2)
                    });
                var pathToEntryAssembly = bootstrapperSettings.EntryAssembly.Location;
                var pathToContentRoot = Path.GetDirectoryName(pathToEntryAssembly);
                var assemblyName = Path.GetFileNameWithoutExtension(pathToEntryAssembly);
                c.IncludeXmlComments(Path.Combine(pathToContentRoot, assemblyName + ".xml"));
                var apiAssemblyName = Path.GetFileNameWithoutExtension(typeof(DefaultController).Assembly.Location);
                c.IncludeXmlComments(Path.Combine(pathToContentRoot, apiAssemblyName + ".xml"));
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();

                var authority = (authSettings.Swagger.SignInEndpoint?.AbsoluteUri?.TrimEnd('/') ?? string.Empty) + '/' + (authSettings.Swagger.Tenant ?? string.Empty);
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme {
                    Flow = "implicit",
                    AuthorizationUrl = $"{authority}/oauth2/authorize"
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public static IServiceCollection AddAllLinksCreators(this IServiceCollection services) {
            return services
                .AddSingleton<IApplicationUriResolver, ApplicationUriResolver>()
                .AddSingleton<IHyperlinkFactory, HyperlinkFactory>()
                .AddSingleton<IApiHomeResponseLinksCreatorFactory, ApiHomeResponseLinksCreatorFactory>()
                .AddSingleton<IUserInfoResponseLinksCreatorFactory, UserInfoResponseLinksCreatorFactory>()
                .AddSingleton<IClaimLinksCreatorFactory, ClaimLinksCreatorFactory>();
        }

        public static IServiceCollection AddPreconfiguredJsonSerializer(this IServiceCollection services) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var jsonSerializer = PreConfiguredJsonSerializer.Create();
            services.AddSingleton(jsonSerializer);

            return services;
        }

        public static IServiceCollection AddFileProviders(this IServiceCollection services) {
            return services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IHostingEnvironment>().WebRootFileProvider);
        }
    }
}