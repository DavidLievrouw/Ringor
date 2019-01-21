using Cake.Frosting;
using Cake.Npm;
using Dalion.Ringor.Build.Tasks.Restore;

namespace Dalion.Ringor.Build.Tasks.Test {
    [TaskName(nameof(UnitTestJS))]
    [Dependency(typeof(InitVersion))]
    [Dependency(typeof(RestorePackages))]
    public sealed class UnitTestJS : FrostingTask<Context> {
        public override void Run(Context context) {
            context.NpmRunScript("test", settings => { settings.WorkingDirectory = context.Ringor.FileSystem.ProjectsAndSolutions.Ringor.ReactAppDirectory; });
        }
    }
}