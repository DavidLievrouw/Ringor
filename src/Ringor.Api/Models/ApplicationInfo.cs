namespace Dalion.Ringor.Api.Models {
    public class ApplicationInfo {
        public string Company { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }
        public ApplicationUrlInfo UrlInfo { get; set; }
        public string Environment { get; set; }

        public class ApplicationUrlInfo {
            public string SiteUrl { get; set; }
            public string AppUrl { get; set; }
        }
    }
}