using System;
using Cake.Core;
using Cake.Core.IO;

namespace Dalion.Ringor.Build.Properties.FileSystem.ProjectsAndSolutions {
    public class RingorProperties : Properties<RingorProperties> {
        private readonly Properties.RingorProperties _container;

        public RingorProperties(ICakeContext context, Properties.RingorProperties container) : base(context) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public DirectoryPath ProjectDirectory => Context.GetAbsoluteDirectoryPath(_container.FileSystem.SourceDirectory + "/Ringor");
        public FilePath ProjectFile => Context.GetAbsoluteFilePath(ProjectDirectory + "/Ringor.csproj");
        public DirectoryPath PublishDirectory => Context.GetAbsoluteDirectoryPath(_container.FileSystem.DistDirectory + "/Publish");
        public DirectoryPath PublishDirectoryAzure => Context.GetAbsoluteDirectoryPath(PublishDirectory + "/Azure");
    }
}