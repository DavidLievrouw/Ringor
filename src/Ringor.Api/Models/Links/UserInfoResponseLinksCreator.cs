using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dalion.Ringor.Api.Models.Links {
    public class UserInfoResponseLinksCreator : ILinksCreator<UserInfoResponse> {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public UserInfoResponseLinksCreator(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public Task CreateLinksFor(UserInfoResponse response) {
            if (response == null) return Task.CompletedTask;

            response.Links = new[] {
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api/userinfo", UserInfoResponseHyperlinkType.Self),
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api", UserInfoResponseHyperlinkType.GetApiRoot)
            };
            
            return Task.CompletedTask;
        }
    }
}