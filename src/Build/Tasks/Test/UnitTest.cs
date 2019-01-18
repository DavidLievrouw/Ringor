using System.IO;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Frosting;
using Dalion.Ringor.Build.Tasks.Restore;

namespace Dalion.Ringor.Build.Tasks.Test {
    [TaskName(nameof(UnitTest))]
    [Dependency(typeof(InitVersion))]
    [Dependency(typeof(RestorePackages))]
    public sealed class UnitTest : FrostingTask<Context> {
        public override void Run(Context context) {
            // Clean directory first
            context.CleanDirectory(context.Ringor.FileSystem.UnitTestTargetDirectory);

            // Find all test projects
            var testProjects = Directory.GetFiles(
                context.Ringor.FileSystem.SourceDirectory.FullPath,
                "*.Tests.csproj",
                SearchOption.AllDirectories);

            // Run all tests
            foreach (var testProject in testProjects) {
                var fileName = Path.GetFileNameWithoutExtension(testProject);
                context.Information($"Running tests for {fileName}: {testProject}...");
                var mainTestSettings = new DotNetCoreTestSettings {
                    Configuration = context.Ringor.Arguments.Configuration,
                    Verbosity = context.Ringor.Arguments.DotNetCoreVerbosity,
                    OutputDirectory = context.Ringor.FileSystem.UnitTestTargetDirectory.FullPath + "/Ringor/" + fileName,
                    Logger = "trx;LogFileName=" + context.Ringor.FileSystem.UnitTestTargetDirectory.FullPath + "\\UnitTest-" + fileName + ".trx",
                    NoRestore = true
                };
                context.DotNetCoreTest(
                    testProject,
                    mainTestSettings);
            }
        }
    }
}