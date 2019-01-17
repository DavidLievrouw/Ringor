using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Startup {
    public static partial class Extensions {
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
    }
}