using System;
using Cake.Core;
using Cake.Core.IO;

namespace Dalion.Ringor.Build.Configuration.FileSystem {
    public class FileSystemProperties : Properties<FileSystemProperties> {
        private readonly AppProperties _container;

        public FileSystemProperties(ICakeContext context, AppProperties container) : base(context) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            ProjectsAndSolutions = new ProjectsAndSolutionsProperties(context, container);
        }

        public DirectoryPath RepoRootDirectory => Context.GetAbsoluteDirectoryPath(".");

        public DirectoryPath SourceDirectory => Context.GetAbsoluteDirectoryPath(RepoRootDirectory + "/src");

        public DirectoryPath AssetsDirectory => Context.GetAbsoluteDirectoryPath(RepoRootDirectory + "/Assets");

        public DirectoryPath DistDirectory => string.IsNullOrWhiteSpace(_container.Arguments.PublishDirectory)
            ? Context.GetAbsoluteDirectoryPath(RepoRootDirectory + "/dist")
            : Context.GetAbsoluteDirectoryPath(_container.Arguments.PublishDirectory);

        public DirectoryPath CSharpUnitTestTargetDirectory => Context.GetAbsoluteDirectoryPath(DistDirectory + "/Test");

        public DirectoryPath ReleaseTargetDirectory => Context.GetAbsoluteDirectoryPath(DistDirectory + "/Release");

        public FilePath VersionFile => Context.GetAbsoluteFilePath(ProjectsAndSolutions.ProjectDirectory + "/version.txt");

        public ProjectsAndSolutionsProperties ProjectsAndSolutions { get; }
    }
}