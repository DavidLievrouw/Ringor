using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Core;
using Cake.Frosting;

namespace Dalion.Ringor.Build.Tasks.Restore {
    [TaskName(nameof(RestorePackagesCSharp))]
    public sealed class RestorePackagesCSharp : FrostingTask<Context> {
        public override void Run(Context context) {
            context.DotNetCoreRestore(
                context.Ringor.FileSystem.ProjectsAndSolutions.ProductSolution.FullPath,
                new DotNetCoreRestoreSettings {
                    IgnoreFailedSources = true,
                    Verbosity = context.Ringor.Arguments.DotNetCoreVerbosity,
                    ArgumentCustomization = args => args.Append("--force")
                });
        }
    }
}