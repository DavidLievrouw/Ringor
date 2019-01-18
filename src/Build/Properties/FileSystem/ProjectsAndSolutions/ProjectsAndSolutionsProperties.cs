using System;
using Cake.Core;
using Cake.Core.IO;

namespace Dalion.Ringor.Build.Properties.FileSystem.ProjectsAndSolutions {
    public class ProjectsAndSolutionsProperties : Properties<ProjectsAndSolutionsProperties> {
        private readonly Properties.RingorProperties _container;

        public ProjectsAndSolutionsProperties(ICakeContext context, Properties.RingorProperties container) : base(context) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            Ringor = new RingorProperties(context, container);
        }

        public FilePath ProductSolution => Context.GetAbsoluteFilePath(_container.FileSystem.SourceDirectory + "/Ringor.sln");

        public RingorProperties Ringor { get; }
    }
}