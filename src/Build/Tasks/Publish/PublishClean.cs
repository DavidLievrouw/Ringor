using Cake.Common.IO;
using Cake.Frosting;

namespace Dalion.Ringor.Build.Tasks.Publish {
    [TaskName(nameof(PublishClean))]
    public sealed class PublishClean : FrostingTask<Context> {
        public override void Run(Context context) {
            context.CleanDirectory(context.Ringor.FileSystem.DistDirectory);
        }
    }
}