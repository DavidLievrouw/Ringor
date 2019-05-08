using System;
using Cake.Core;
using Cake.Core.IO;

namespace Dalion.Ringor.Build.Configuration.FileSystem {
    public class ProjectsAndSolutionsProperties : Properties<ProjectsAndSolutionsProperties> {
        private readonly AppProperties _container;

        public ProjectsAndSolutionsProperties(ICakeContext context, AppProperties container) : base(context) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public FilePath ProductSolution => Context.GetAbsoluteFilePath(_container.FileSystem.SourceDirectory + "/Ringor.sln");
        public DirectoryPath ProjectDirectory => Context.GetAbsoluteDirectoryPath(_container.FileSystem.SourceDirectory + "/Ringor");
        public DirectoryPath ReactAppDirectory => Context.GetAbsoluteDirectoryPath(ProjectDirectory + "/App");
        public FilePath ProjectFile => Context.GetAbsoluteFilePath(ProjectDirectory + "/Ringor.csproj");
        public DirectoryPath PublishDirectoryAzure => Context.GetAbsoluteDirectoryPath(_container.FileSystem.PublishDirectory + "/Azure");
        public DirectoryPath PublishDirectoryWebDeploy => Context.GetAbsoluteDirectoryPath(_container.FileSystem.PublishDirectory + "/WebDeploy");
    }
}