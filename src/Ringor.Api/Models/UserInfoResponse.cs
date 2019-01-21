using Dalion.Ringor.Api.Models.Links;

namespace Dalion.Ringor.Api.Models {
    public class UserInfoResponse : ILinkableResource<UserInfoResponseHyperlinkType> {
        public Claim[] Claims { get; set; }
        public Hyperlink<UserInfoResponseHyperlinkType>[] Links { get; set; }
    }
}