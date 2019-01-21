using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Dalion.Ringor.Startup {
    public class FakeHostingEnvironment : IHostingEnvironment {
        public string EnvironmentName { get; set; } = Microsoft.AspNetCore.Hosting.EnvironmentName.Development;
        public string ApplicationName { get; set; } = "Ringor";
        public string WebRootPath { get; set; } = Path.GetDirectoryName(typeof(Program).Assembly.Location);
        public IFileProvider WebRootFileProvider { get; set; } = new PhysicalFileProvider(Path.GetDirectoryName(typeof(Program).Assembly.Location));
        public string ContentRootPath { get; set; } = Path.GetDirectoryName(typeof(Program).Assembly.Location);
        public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(Path.GetDirectoryName(typeof(Program).Assembly.Location));
    }
}