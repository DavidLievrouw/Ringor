using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Dalion.Ringor.Startup {
    public class FakeHostingEnvironment : IHostingEnvironment {
        private static readonly Assembly MainAssembly;

        static FakeHostingEnvironment() {
            MainAssembly = typeof(Program).Assembly;
        }

        public string EnvironmentName { get; set; } = Microsoft.AspNetCore.Hosting.EnvironmentName.Development;
        public string ApplicationName { get; set; } = MainAssembly.GetName().Name;
        public string WebRootPath { get; set; } = Path.GetDirectoryName(MainAssembly.Location);
        public IFileProvider WebRootFileProvider { get; set; } = new PhysicalFileProvider(Path.GetDirectoryName(MainAssembly.Location));
        public string ContentRootPath { get; set; } = Path.GetDirectoryName(MainAssembly.Location);
        public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(Path.GetDirectoryName(MainAssembly.Location));
    }
}