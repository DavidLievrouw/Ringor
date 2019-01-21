using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApiHomeResponseLinksCreator : ILinksCreator<ApiHomeResponse> {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public ApiHomeResponseLinksCreator(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public Task CreateLinksFor(ApiHomeResponse response) {
            if (response == null) return Task.CompletedTask;

            response.Links = new[] {
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api", ApiHomeResponseHyperlinkType.Self),
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api/userinfo", ApiHomeResponseHyperlinkType.GetUserInfo)
            };
            
            return Task.CompletedTask;
        }
    }
}