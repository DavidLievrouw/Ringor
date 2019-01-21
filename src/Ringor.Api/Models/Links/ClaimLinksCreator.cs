using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dalion.Ringor.Api.Models.Links {
    public class ClaimLinksCreator : ILinksCreator<Claim> {
        private readonly IHyperlinkFactory _hyperlinkFactory;

        public ClaimLinksCreator(IHyperlinkFactory hyperlinkFactory) {
            _hyperlinkFactory = hyperlinkFactory ?? throw new ArgumentNullException(nameof(hyperlinkFactory));
        }

        public Task CreateLinksFor(Claim claim) {
            if (claim == null) return Task.CompletedTask;

            claim.Links = new[] {
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api/userinfo/{claim.Type}", ClaimHyperlinkType.EnumerateAllClaimsOfThisType),
                _hyperlinkFactory.Create(HttpMethod.Get, $"/api/userinfo", ClaimHyperlinkType.GetUserInfo)
            };
            
            return Task.CompletedTask;
        }
    }
}