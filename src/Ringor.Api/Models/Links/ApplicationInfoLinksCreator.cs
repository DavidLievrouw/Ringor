using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationInfoLinksCreator : ILinksCreator<ApplicationInfo> {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public ApplicationInfoLinksCreator(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public Task CreateLinksFor(ApplicationInfo checks) {
            if (checks == null) return Task.CompletedTask;

            checks.Links = new[] {
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api", ApplicationInfoHyperlinkType.Self),
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api/userinfo", ApplicationInfoHyperlinkType.GetUserInfo)
            };
            
            return Task.CompletedTask;
        }
    }
}