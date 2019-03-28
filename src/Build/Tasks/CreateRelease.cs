using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Compression;
using Cake.Frosting;

namespace Dalion.Ringor.Build.Tasks {
    [TaskName(nameof(CreateRelease))]
    [Dependency(typeof(InitVersion))]
    [Dependency(typeof(Publish.Publish))]
    public sealed class CreateRelease : FrostingTask<Context> {
        public override void Run(Context context) {
            context.CleanDirectory(context.App.FileSystem.ReleaseTargetDirectory);

            // Compress published targets
            context.Debug("Compressing Azure application...");
            context.ZipCompress(
                context.App.FileSystem.ProjectsAndSolutions.PublishDirectoryAzure, 
                context.App.FileSystem.ReleaseTargetDirectory + "/AzureApplication.zip");

            context.Information($"Release archive created at: {context.App.FileSystem.ReleaseTargetDirectory}");
        }
    }
}