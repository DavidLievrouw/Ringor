using System;

namespace Dalion.Ringor.Configuration {
    public class SftpSettings {
        public Uri Url { get; set; }
        public string Directory { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}