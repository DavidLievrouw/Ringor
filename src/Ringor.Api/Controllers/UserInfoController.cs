using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Models.Links;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Api.Controllers {
    [Route("api/userinfo")]
    [Authorize]
    public class UserInfoController : Controller {
        private readonly IClaimLinksCreatorFactory _claimLinksCreatorFactory;
        private readonly IUserInfoResponseLinksCreatorFactory _userInfoResponseLinksCreatorFactory;

        public UserInfoController(
            IUserInfoResponseLinksCreatorFactory userInfoResponseLinksCreatorFactory,
            IClaimLinksCreatorFactory claimLinksCreatorFactory) {
            _userInfoResponseLinksCreatorFactory = userInfoResponseLinksCreatorFactory ?? throw new ArgumentNullException(nameof(userInfoResponseLinksCreatorFactory));
            _claimLinksCreatorFactory = claimLinksCreatorFactory ?? throw new ArgumentNullException(nameof(claimLinksCreatorFactory));
        }

        /// <summary>
        ///     Get the claims of the current authenticated user.
        /// </summary>
        /// <returns>The claims of the current authenticated user.</returns>
        /// <response code="200">Returns the claims of the current authenticated user.</response>
        [HttpGet("")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserInfoResponse), 200)]
        public async Task<IActionResult> GetUserClaims() {
            var response = new UserInfoResponse {
                Claims = User.Claims.Select(c => new Claim {
                    Type = c.Type,
                    Value = c.Value
                }).ToArray()
            };

            var linksCreator = _claimLinksCreatorFactory.Create();
            await linksCreator.CreateLinksFor(response.Claims);

            await _userInfoResponseLinksCreatorFactory.Create().CreateLinksFor(response);

            return Ok(response);
        }

        /// <summary>
        ///     Get the claims of the specified type of the current authenticated user.
        /// </summary>
        /// <param name="claimType">The type of the claims to query.</param>
        /// <returns>The claims of the current authenticated user of the specified type.</returns>
        /// <response code="200">Returns the claims of the current authenticated user of the specified type.</response>
        /// <response code="404">When the authenticated user has no claims of the specified type.</response>
        [HttpGet("{claimType}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Claim>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserClaimsByType(string claimType) {
            if (string.IsNullOrEmpty(claimType)) return NotFound();

            claimType = WebUtility.UrlDecode(claimType);

            var allClaimValues = User.Claims
                .Select(c => new {
                    c.Type,
                    c.Value
                })
                .Where(c => c.Type == claimType)
                .Select(c => c.Value)
                .ToList();
            if (!allClaimValues.Any()) return NotFound();

            var linksCreator = _claimLinksCreatorFactory.Create();
            var claims = allClaimValues.Select(_ => new Claim {Type = claimType, Value = _}).ToList();
            await claims.ForEachAsync(c => linksCreator.CreateLinksFor(c));

            return Ok(claims);
        }
    }
}