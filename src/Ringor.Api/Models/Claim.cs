using Dalion.Ringor.Api.Models.Links;

namespace Dalion.Ringor.Api.Models {
    public class Claim : ILinkableResource<ClaimHyperlinkType> {
        public string Type { get; set; }
        public string Value { get; set; }
        public Hyperlink<ClaimHyperlinkType>[] Links { get; set; }
    }
}