using System.Reflection;

namespace Dalion.Ringor.Startup {
    internal class BootstrapperSettings {
        public string EnvironmentName { get; set; }
        public bool UseDetailedErrors { get; set; }
        public bool DisableHttpsRedirection { get; set; }
        public Assembly EntryAssembly { get; set; }
    }
}