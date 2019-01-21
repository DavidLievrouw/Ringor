namespace Dalion.Ringor.Api.Models {
    public class ApplicationInfo {
        public string Version { get; set; }
        public ApplicationUrlInfo UrlInfo { get; set; }
        public string Message { get; set; }

        public class ApplicationUrlInfo {
            public string SiteUrl { get; set; }
            public string AppUrl { get; set; }
        }
    }
}