using System;
using System.Reflection;
using Dalion.Ringor.Api.Models;
using Microsoft.AspNetCore.Http;

namespace Dalion.Ringor.Api.Services {
    public class ApplicationInfoProvider : IApplicationInfoProvider {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Assembly _entryAssembly;
        private readonly string _environment;
        private readonly ImplicitFlowAuthenticationSettings _authenticationSettings;

        public ApplicationInfoProvider(
            IHttpContextAccessor httpContextAccessor, 
            Assembly entryAssembly, 
            string environment,
            ImplicitFlowAuthenticationSettings authenticationSettings) {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _entryAssembly = entryAssembly ?? throw new ArgumentNullException(nameof(entryAssembly));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _authenticationSettings = authenticationSettings ?? throw new ArgumentNullException(nameof(authenticationSettings));
        }

        public ApplicationInfo Provide() {
            var applicationInfo = new ApplicationInfo {
                Version = _entryAssembly.GetName().Version.ToString(fieldCount: 3),
                UrlInfo = new ApplicationInfo.ApplicationUrlInfo {
                    SiteUrl = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value,
                    AppUrl = _httpContextAccessor.HttpContext.Request.PathBase
                },
                Company = _entryAssembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company,
                Product = _entryAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product,
                Environment = _environment,
                AuthenticationInfo = new ApplicationInfo.ApplicationAuthenticationInfo {
                    ClientId = _authenticationSettings.ClientId,
                    Tenant = _authenticationSettings.Tenant,
                    Authority = _authenticationSettings.Authority,
                    Scopes = _authenticationSettings.Scopes
                }
            };
            
            return applicationInfo;
        }
    }
}