using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore.Clean;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Frosting;
using Dalion.Ringor.Build.Tasks.Restore;

namespace Dalion.Ringor.Build.Tasks.Publish {
    [TaskName(nameof(PublishWebDeploy))]
    [Dependency(typeof(RestorePackages))]
    public sealed class PublishWebDeploy : FrostingTask<Context> {
        public override void Run(Context context) {
            var msBuildSettings = new DotNetCoreMSBuildSettings();
            msBuildSettings.Properties.Add("PublishEnvironment", new[] {context.App.Arguments.Environment});
            msBuildSettings.Properties.Add("IsPublishing", new[] {"true"});
            msBuildSettings.Properties.Add("DeployOnBuild", new[] {"true"});
            msBuildSettings.Properties.Add("WebPublishMethod", new[] {"Package"});
            msBuildSettings.Properties.Add("PackageLocation", new[] {context.App.FileSystem.ProjectsAndSolutions.PublishDirectoryWebDeploy.FullPath});
            msBuildSettings.Properties.Add("PackageAsSingleFile", new[] {"true"});
            msBuildSettings.NoLogo = true;
            
            context.CleanDirectory(context.App.FileSystem.ProjectsAndSolutions.PublishDirectoryWebDeploy);
            context.DotNetCoreClean(
                context.App.FileSystem.ProjectsAndSolutions.ProjectFile.FullPath,
                new DotNetCoreCleanSettings {
                    Verbosity = context.App.Arguments.DotNetCoreVerbosity,
                    Configuration = context.App.Arguments.Configuration
                });
            context.DotNetCoreBuild(
                context.App.FileSystem.ProjectsAndSolutions.ProjectFile.FullPath,
                new DotNetCoreBuildSettings {
                    Configuration = context.App.Arguments.Configuration,
                    MSBuildSettings = msBuildSettings,
                    Verbosity = context.App.Arguments.DotNetCoreVerbosity,
                    NoRestore = true
                });
        }
    }
}