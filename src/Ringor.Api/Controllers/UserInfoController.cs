using System.Collections.Generic;
using System.Linq;
using System.Net;
using Dalion.Ringor.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Api.Controllers {
    [Route("api/userinfo")]
    [Authorize]
    public class UserInfoController : Controller {
        [HttpGet("")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Claim>), 200)]
        public IActionResult GetUserClaims() {
            return Ok(User.Claims.Select(c => new Claim {
                Type = c.Type,
                Value = c.Value
            }));
        }

        [HttpGet("{claimType}")]
        [Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Claim>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetUserClaimsByType(string claimType) {
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

            return Ok(allClaimValues.Select(_ => new Claim {Type = claimType, Value = _}));
        }
    }
}