using Cake.Core;
using Dalion.Ringor.Build.Configuration.Arguments;
using Dalion.Ringor.Build.Configuration.FileSystem;

namespace Dalion.Ringor.Build.Configuration {
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