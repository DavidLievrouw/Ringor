using Dalion.Ringor.Api.Models.Links;

namespace Dalion.Ringor.Api.Models {
    public class ApiHomeResponse : ILinkableResource<ApiHomeResponseHyperlinkType> {
        public ApplicationInfo ApplicationInfo { get; set; }
        public Hyperlink<ApiHomeResponseHyperlinkType>[] Links { get; set; }
    }
}