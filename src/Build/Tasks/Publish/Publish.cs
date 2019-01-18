using Cake.Frosting;

namespace Dalion.Ringor.Build.Tasks.Publish {
    [TaskName(nameof(Publish))]
    [Dependency(typeof(InitVersion))]
    [Dependency(typeof(PublishClean))]
    [Dependency(typeof(PublishAzure))]
    public sealed class Publish : FrostingTask<Context> { }
}