using Cake.Frosting;
using Cake.Npm;
using Cake.Npm.RunScript;
using Dalion.Ringor.Build.Tasks.Restore;

namespace Dalion.Ringor.Build.Tasks.Test {
    [TaskName(nameof(UnitTestJs))]
    [Dependency(typeof(InitVersion))]
    [Dependency(typeof(RestorePackages))]
    public sealed class UnitTestJs : FrostingTask<Context> {
        public override void Run(Context context) {
            void Configurator(NpmRunScriptSettings settings) {
                settings.WorkingDirectory = context.App.FileSystem.ProjectsAndSolutions.ReactAppDirectory;
            }

            context.NpmRunScript("tests", Configurator);
        }
    }
}