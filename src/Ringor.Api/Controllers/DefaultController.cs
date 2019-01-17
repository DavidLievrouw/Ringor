using System;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Api.Controllers {
    [Route("api")]
    [Authorize]
    public class DefaultController : Controller {
        private readonly IApplicationInfoProvider _applicationInfoProvider;

        public DefaultController(IApplicationInfoProvider applicationInfoProvider) {
            _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
        }

        [HttpGet("")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApplicationInfo), 200)]
        public IActionResult GetDefault() {
            return Ok(_applicationInfoProvider.Provide());
        }
    }
}