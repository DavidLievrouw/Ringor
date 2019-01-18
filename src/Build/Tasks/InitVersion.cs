using Cake.Common.Diagnostics;
using Cake.FileHelpers;
using Cake.Frosting;

namespace Dalion.Ringor.Build.Tasks {
    [TaskName(nameof(InitVersion))]
    public sealed class InitVersion : FrostingTask<Context> {
        public override void Run(Context context) {
            var productVersion = context.FileReadText(context.Ringor.FileSystem.VersionFile);
            var assemblyVersion = $"{productVersion}.0";

            context.Information("Product version       = " + productVersion);
            context.Information("Assembly version      = " + assemblyVersion);

            // Set versions in Context
            context.Ringor.ProductVersion = productVersion;
            context.Ringor.AssemblyVersion = assemblyVersion;
        }
    }
}