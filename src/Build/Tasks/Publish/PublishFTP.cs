using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Clean;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Frosting;
using Dalion.Ringor.Build.Tasks.Restore;

namespace Dalion.Ringor.Build.Tasks.Publish {
    [TaskName(nameof(PublishFTP))]
    [Dependency(typeof(RestorePackages))]
    public sealed class PublishFTP : FrostingTask<Context> {
        public override void Run(Context context) {
            var msBuildSettings = new DotNetCoreMSBuildSettings();
            msBuildSettings.Properties.Add("PublishEnvironment", new[] {context.App.Arguments.Environment});
            msBuildSettings.Properties.Add("IsPublishing", new[] {"true"});
            msBuildSettings.NoLogo = true;

            context.CleanDirectory(context.App.FileSystem.ProjectsAndSolutions.PublishDirectoryFTP);
            context.DotNetCoreClean(
                context.App.FileSystem.ProjectsAndSolutions.ProjectFile.FullPath,
                new DotNetCoreCleanSettings {
                    Verbosity = context.App.Arguments.DotNetCoreVerbosity,
                    Configuration = context.App.Arguments.Configuration
                });
            context.DotNetCorePublish(
                context.App.FileSystem.ProjectsAndSolutions.ProjectFile.FullPath,
                new DotNetCorePublishSettings {
                    Configuration = context.App.Arguments.Configuration,
                    OutputDirectory = context.App.FileSystem.ProjectsAndSolutions.PublishDirectoryFTP,
                    MSBuildSettings = msBuildSettings,
                    Verbosity = context.App.Arguments.DotNetCoreVerbosity,
                    NoRestore = true
                });
        }
    }
}