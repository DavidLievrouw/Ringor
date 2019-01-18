using Cake.Core;
using Dalion.Ringor.Build.Properties.Arguments;
using Dalion.Ringor.Build.Properties.FileSystem;

namespace Dalion.Ringor.Build.Properties {
    public class RingorProperties : Properties<RingorProperties> {
        public RingorProperties(ICakeContext context) : base(context) {
            Arguments = new ArgumentsProperties(context, this);
            FileSystem = new FileSystemProperties(context, this);
            WorkingDirectory = context.Environment.WorkingDirectory.FullPath;
        }

        public string WorkingDirectory { get; }
        public string ProductName { get; } = "Dalion.Ringor";
        public string ProductVersion { get; set; } = "1.0.0";
        public string AssemblyVersion { get; set; } = "1.0.0.0";

        public ArgumentsProperties Arguments { get; }
        public FileSystemProperties FileSystem { get; }
    }
}