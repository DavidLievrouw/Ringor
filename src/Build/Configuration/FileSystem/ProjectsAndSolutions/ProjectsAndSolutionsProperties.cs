using System;
using Cake.Core;
using Cake.Core.IO;

namespace Dalion.Ringor.Build.Configuration.FileSystem.ProjectsAndSolutions {
    public class ProjectsAndSolutionsProperties : Properties<ProjectsAndSolutionsProperties> {
        private readonly Configuration.RingorProperties _container;

        public ProjectsAndSolutionsProperties(ICakeContext context, Configuration.RingorProperties container) : base(context) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            Ringor = new RingorProperties(context, container);
        }

        public FilePath ProductSolution => Context.GetAbsoluteFilePath(_container.FileSystem.SourceDirectory + "/Ringor.sln");

        public RingorProperties Ringor { get; }
    }
}