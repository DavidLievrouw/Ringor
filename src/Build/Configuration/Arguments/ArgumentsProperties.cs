using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Frosting;
using Dalion.Ringor.Build.Startup;

namespace Dalion.Ringor.Build.Configuration.Arguments {
    public class ArgumentsProperties : Properties<ArgumentsProperties> {
        private readonly AppProperties _container;

        public ArgumentsProperties(ICakeContext context, AppProperties container) : base(context) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public string Target => Context.Arguments.HasArgument("target") ? Context.Arguments.GetArgument("target") : "Default";
        public string Configuration => Context.Arguments.HasArgument("configuration") ? Context.Arguments.GetArgument("configuration") : "Release";
        public string Environment => Context.Arguments.HasArgument("environment") ? Context.Arguments.GetArgument("environment") : "Production";
        public string PublishDirectory => Context.Arguments.HasArgument("publishDirectory") ? Context.Arguments.GetArgument("publishDirectory") : null;

        public DotNetCoreVerbosity DotNetCoreVerbosity {
            get {
                var verbosityArg = Context.Arguments.GetArgument("verbosity")?.ToLower() ?? "";
                var verbosityDictionary = new Dictionary<string, DotNetCoreVerbosity>(StringComparer.InvariantCultureIgnoreCase) {
                    {VerbosityOptions.Diagnostic.ToString().ToLower(), DotNetCoreVerbosity.Diagnostic},
                    {VerbosityOptions.Verbose.ToString().ToLower(), DotNetCoreVerbosity.Detailed},
                    {VerbosityOptions.Normal.ToString().ToLower(), DotNetCoreVerbosity.Normal},
                    {VerbosityOptions.Minimal.ToString().ToLower(), DotNetCoreVerbosity.Minimal},
                    {VerbosityOptions.Quiet.ToString().ToLower(), DotNetCoreVerbosity.Quiet}
                };
                if (!verbosityDictionary.TryGetValue(verbosityArg, out var dotNetCoreVerbosity)) {
                    throw new FrostingException($"Value '{verbosityArg}' for {nameof(DotNetCoreVerbosity)} is invalid.");
                }
                return dotNetCoreVerbosity;
            }
        }
    }
}