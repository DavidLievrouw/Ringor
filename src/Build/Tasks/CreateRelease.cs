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
            context.CleanDirectory(context.Ringor.FileSystem.ReleaseTargetDirectory);

            // Compress published targets
            context.Debug("Compressing Azure application...");
            context.ZipCompress(
                context.Ringor.FileSystem.ProjectsAndSolutions.Ringor.PublishDirectoryAzure, 
                context.Ringor.FileSystem.ReleaseTargetDirectory + "/AzureApplication.zip");

            context.Information($"Release archive created at: {context.Ringor.FileSystem.ReleaseTargetDirectory}");
        }
    }
}