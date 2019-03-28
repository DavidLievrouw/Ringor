using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Clean;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Core;
using Cake.Frosting;
using Dalion.Ringor.Build.Tasks.Restore;

namespace Dalion.Ringor.Build.Tasks.Publish {
    [TaskName(nameof(PublishAzureFiles))]
    [Dependency(typeof(RestorePackages))]
    public sealed class PublishAzureFiles : FrostingTask<Context> {
        public override void Run(Context context) {
            var msBuildSettings = new DotNetCoreMSBuildSettings();
            msBuildSettings.Properties.Add("PublishEnvironment", new[] {context.App.Arguments.Environment});
            msBuildSettings.Properties.Add("PublishProfile", new[] {"Properties\\PublishProfiles\\AzureFiles.pubxml"});

            // Suppress warning for conflicting dependency versions. 
            // This warning is not applicable for NETSTANDARD projects, apparently.
            msBuildSettings.WarningCodesAsMessage.Add("MSB3277");

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
                    OutputDirectory = context.App.FileSystem.ProjectsAndSolutions.PublishDirectoryAzure,
                    MSBuildSettings = msBuildSettings,
                    Verbosity = context.App.Arguments.DotNetCoreVerbosity,
                    ArgumentCustomization = args => args.Append("--no-restore")
                });
        }
    }
}