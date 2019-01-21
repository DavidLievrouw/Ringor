using Cake.Frosting;

namespace Dalion.Ringor.Build.Tasks.Restore {
    [TaskName(nameof(RestorePackages))]
    [Dependency(typeof(RestorePackagesCSharp))]
    [Dependency(typeof(RestorePackagesNpm))]
    public sealed class RestorePackages : FrostingTask<Context> { }
}