using System;
using System.IO;
using System.Linq;
using Dalion.Ringor.Api.Controllers;
using Dalion.Ringor.Api.Models.Links;
using Dalion.Ringor.Api.Serialization;
using Dalion.Ringor.Api.Services;
using Dalion.Ringor.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Dalion.Ringor.Startup {
    public class CompositionTests : IDisposable {
        private readonly ServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public CompositionTests() {
            // Load real configuration
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location))
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", false, true)
                .Build();

            // Build services
            var serviceCollection = new ServiceCollection();
            var bootstrapperSettings = new BootstrapperSettings {
                EntryAssembly = typeof(Program).Assembly,
                EnvironmentName = "",
                UseDetailedErrors = true
            };
            var hostingEnvironment = new FakeHostingEnvironment();
            Composition.ConfigureServices(serviceCollection, hostingEnvironment, _configuration, bootstrapperSettings);
            
            // Add registrations that are performed by the WebHostStartup
            serviceCollection
                .AddMvc()
                .AddApplicationPart(typeof(DefaultController).Assembly)
                .AddControllersAsServices();

            // Build service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose() {
            _serviceProvider?.Dispose();
        }

        [Fact]
        public void CanRegisterAllMvcControllers() {
            var controllerAssembly = typeof(DefaultController).Assembly;
            var controllers = controllerAssembly.GetTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();
            controllers.ForEach(c => {
                var instance = _serviceProvider.GetRequiredService(c);
                instance.Should().NotBeNull().And.BeAssignableTo(c);
            });
        }

        [Theory]
        [InlineData(typeof(IApiHomeResponseLinksCreatorFactory))]
        [InlineData(typeof(IUserInfoResponseLinksCreatorFactory))]
        [InlineData(typeof(IClaimLinksCreatorFactory))]
        [InlineData(typeof(IApplicationUriResolver))]
        [InlineData(typeof(IJsonSerializer))]
        [InlineData(typeof(IApplicationInfoProvider))]
        public void CanResolveType(Type requestedType) {
            var instance = _serviceProvider.GetRequiredService(requestedType);
            instance.Should().NotBeNull().And.BeAssignableTo(requestedType);
        }

        [Theory]
        [InlineData(typeof(IOptions<AuthenticationSettings>))]
        [InlineData(typeof(IOptions<RingSettings>))]
        [InlineData(typeof(IOptions<SftpSettings>))]
        [InlineData(typeof(AuthenticationSettings))]
        [InlineData(typeof(RingSettings))]
        [InlineData(typeof(SftpSettings))]
        public void CanResolveOptions(Type requestedOptionsType) {
            var instance = _serviceProvider.GetService(requestedOptionsType);
            instance.Should().NotBeNull().And.BeAssignableTo(requestedOptionsType);
        }

        //[Theory]
        //[InlineData(typeof(IAccessTokenAcquirer), typeof(CachingAccessTokenAcquirer))]
        //public void CanRegisterDecorators(Type requestedType, Type expectedType) {
        //    var instance = _serviceProvider.GetService(requestedType);
        //    instance.Should().NotBeNull().And.BeOfType(expectedType);
        //}
    }
}