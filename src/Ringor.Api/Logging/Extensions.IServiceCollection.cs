using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dalion.Ringor.Api.Logging {
    public static partial class Extensions {
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
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            Log.Logger = logger;

            return services;
        }
    }
}