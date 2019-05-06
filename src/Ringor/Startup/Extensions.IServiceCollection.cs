using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dalion.Ringor.Api.Controllers;
using Dalion.Ringor.Api.Models.Links;
using Dalion.Ringor.Api.Security;
using Dalion.Ringor.Api.Services;
using Dalion.Ringor.Api.Swagger;
using Dalion.Ringor.Configuration;
using Dalion.Ringor.Logging;
using Dalion.Ringor.Serialization;
using Dalion.Ringor.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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
                    prov.GetRequiredService<BootstrapperSettings>().EnvironmentName,
                    prov.GetRequiredService<ImplicitFlowAuthenticationSettings>()));
        }

        public static IServiceCollection AddAzureAdAuthentication(this IServiceCollection services, AuthenticationSettings authSettings) {
            if (authSettings == null) throw new ArgumentNullException(nameof(authSettings));

            services
                .AddSingleton(serviceProvider => new ImplicitFlowAuthenticationSettings {
                    Tenant = authSettings.Tenant,
                    ClientId = authSettings.ClientId,
                    Authority = authSettings.SignInEndpoint.WithRelativePath(authSettings.Tenant),
                    Scopes = authSettings.Scopes?.Distinct().ToArray()
                })
                .AddAuthorization(options => {
                    options.AddPolicy(AuthorizationPolicies.RequireApiAccess, policy => policy.RequireClaim(ClaimTypes.Scope, "Api.FullAccess"));
                    options.DefaultPolicy = options.GetPolicy(AuthorizationPolicies.RequireApiAccess);
                })
                .AddAuthentication(o => { o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
                .AddJwtBearer(o => {
                    o.Authority = authSettings.SignInEndpoint.WithRelativePath(authSettings.Tenant).AbsoluteUri;
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
                        ValidAudiences = new [] {
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
            return services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen(options => {
                    var pathToEntryAssembly = bootstrapperSettings.EntryAssembly.Location;
                    var pathToContentRoot = Path.GetDirectoryName(pathToEntryAssembly);
                    var assemblyName = Path.GetFileNameWithoutExtension(pathToEntryAssembly);
                    options.IncludeXmlComments(Path.Combine(pathToContentRoot, assemblyName + ".xml"));
                    var apiAssemblyName = Path.GetFileNameWithoutExtension(typeof(DefaultController).Assembly.Location);
                    options.IncludeXmlComments(Path.Combine(pathToContentRoot, apiAssemblyName + ".xml"));
                    options.DescribeAllEnumsAsStrings();
                    options.DescribeStringEnumsInCamelCase();

                    var authority = authSettings.SignInEndpoint.WithRelativePath(authSettings.Tenant);
                    options.AddSecurityDefinition("oauth2", new OAuth2Scheme {
                        Flow = "implicit",
                        AuthorizationUrl = $"{authority}/oauth2/authorize"
                    });
                    options.OperationFilter<AuthorizeCheckOperationFilter>();
                    options.OperationFilter<SwaggerDefaultValues>();
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

        public static IServiceCollection AddApiVersioning(this IServiceCollection services) {
            return services
                .AddApiVersioning(options => {
                    options.UseApiBehavior = true; // Apply only on controllers with ApiControllerAttribute
                    options.AssumeDefaultVersionWhenUnspecified = true; // Don't require clients to specify which version they want
                    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options); // When not specifying a version, use the latest version
                    options.ReportApiVersions = true; // Report supported API versions for requests to controllers having an ApiControllerAttribute
                    options.ApiVersionReader = new MediaTypeApiVersionReader(); // Versions are specified using the Accept-header
                });
        }

        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration) {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return AddSerilog(services, new LoggerConfiguration().ReadFrom.Configuration(configuration));
        }

        public static IServiceCollection AddSerilog(this IServiceCollection services, LoggerConfiguration configuration) {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var logger = configuration.CreateLogger();

            services.AddSingleton<ILogger>(logger);
            services.AddSingleton(typeof(ILogger<>), typeof(SerilogLogger<>));
            Log.Logger = logger;

            return services;
        }

        public static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services) {
            return services.Configure<CookiePolicyOptions>(options => {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
        }
    }
}