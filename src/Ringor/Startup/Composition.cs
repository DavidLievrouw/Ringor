using System.Collections.Generic;
using Dalion.Ringor.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Startup {
    internal static class Composition {
        public static void ConfigureServices(IServiceCollection services, WebHostBuilderContext context, IConfiguration configuration) {
            // Configuration
            var ringSettings = services.ConfigureSettings<RingSettings>(configuration.GetSection("RingSettings"));
            var sftpSettings = services.ConfigureSettings<SftpSettings>(configuration.GetSection("SftpSettings"));
            var authSettings = services.ConfigureSettings<AuthenticationSettings>(configuration.GetSection("Authentication"));

            // Authentication
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
        }
    }
}