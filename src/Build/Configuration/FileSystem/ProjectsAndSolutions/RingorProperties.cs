using System;
using Cake.Core;
using Cake.Core.IO;

namespace Dalion.Ringor.Build.Configuration.FileSystem.ProjectsAndSolutions {
    public class RingorProperties : Properties<RingorProperties> {
        private readonly Configuration.RingorProperties _container;

        public RingorProperties(ICakeContext context, Configuration.RingorProperties container) : base(context) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public DirectoryPath ProjectDirectory => Context.GetAbsoluteDirectoryPath(_container.FileSystem.SourceDirectory + "/Ringor");
        public DirectoryPath ReactAppDirectory => Context.GetAbsoluteDirectoryPath(ProjectDirectory + "/App");
        public FilePath ProjectFile => Context.GetAbsoluteFilePath(ProjectDirectory + "/Ringor.csproj");
        public DirectoryPath PublishDirectory => Context.GetAbsoluteDirectoryPath(_container.FileSystem.DistDirectory + "/Publish");
        public DirectoryPath PublishDirectoryAzure => Context.GetAbsoluteDirectoryPath(PublishDirectory + "/Azure");
    }
}