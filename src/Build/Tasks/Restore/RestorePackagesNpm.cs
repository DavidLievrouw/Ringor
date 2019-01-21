using Cake.Frosting;
using Cake.Npm;
using Cake.Npm.Install;

namespace Dalion.Ringor.Build.Tasks.Restore {
    [TaskName(nameof(RestorePackagesNpm))]
    public sealed class RestorePackagesNpm : FrostingTask<Context> {
        public override void Run(Context context) {
            void Configurator(NpmInstallSettings settings) {
                settings.WorkingDirectory = context.Ringor.FileSystem.ProjectsAndSolutions.Ringor.ReactAppDirectory;
            }

            context.NpmInstall(Configurator);
        }
    }
}