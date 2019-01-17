using Microsoft.AspNetCore.Hosting;

namespace Dalion.Ringor.Startup {
    internal static partial class Extensions {
        public static bool IsDevelopmentOrDebug(this IHostingEnvironment env) {
            // ReSharper disable once RedundantAssignment
            var isDevelopmentOrDebug = env.IsDevelopment();

#if DEBUG
            isDevelopmentOrDebug = true;
#endif

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            return isDevelopmentOrDebug;
        }
    }
}