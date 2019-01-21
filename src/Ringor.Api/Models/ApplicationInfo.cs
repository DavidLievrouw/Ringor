using Dalion.Ringor.Api.Models.Links;

namespace Dalion.Ringor.Api.Models {
    public class ApplicationInfo : ILinkableResource<ApplicationInfoHyperlinkType> {
        public string Version { get; set; }
        public ApplicationUrlInfo UrlInfo { get; set; }
        public string Message { get; set; }

        public class ApplicationUrlInfo {
            public string SiteUrl { get; set; }
            public string AppUrl { get; set; }
        }

        public Hyperlink<ApplicationInfoHyperlinkType>[] Links { get; set; }
    }
}