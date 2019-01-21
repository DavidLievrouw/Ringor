using Cake.Frosting;
using Dalion.Ringor.Build.Tasks.Restore;

namespace Dalion.Ringor.Build.Tasks.Test {
    [TaskName(nameof(UnitTest))]
    [Dependency(typeof(InitVersion))]
    [Dependency(typeof(RestorePackages))]
    [Dependency(typeof(UnitTestCSharp))]
    [Dependency(typeof(UnitTestJS))]
    public sealed class UnitTest : FrostingTask<Context> { }
}