using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace Dalion.Ringor.Startup {
    internal class EnvironmentResolver {
        public string ResolveEnvironment(string[] args) {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            const string cmdLineArgPrefix = "--Environment=";
            if (string.IsNullOrWhiteSpace(environment) && args.Any(arg => arg.StartsWith(cmdLineArgPrefix, StringComparison.InvariantCultureIgnoreCase))) {
                var environmentArg = args.First(arg => arg.StartsWith(cmdLineArgPrefix, StringComparison.InvariantCultureIgnoreCase));
                environment = environmentArg.Substring(cmdLineArgPrefix.Length);
            }
            if (string.IsNullOrWhiteSpace(environment)) {
                environment = EnvironmentName.Production;
            }
            return environment;
        }
    }
}