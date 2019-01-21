using System;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Models.Links;
using Dalion.Ringor.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Api.Controllers {
    [Route("api")]
    [AllowAnonymous]
    public class DefaultController : Controller {
        private readonly IApplicationInfoProvider _applicationInfoProvider;
        private readonly IApplicationInfoLinksCreatorFactory _applicationInfoLinksCreatorFactory;

        public DefaultController(IApplicationInfoProvider applicationInfoProvider, IApplicationInfoLinksCreatorFactory applicationInfoLinksCreatorFactory) {
            _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
            _applicationInfoLinksCreatorFactory = applicationInfoLinksCreatorFactory ?? throw new ArgumentNullException(nameof(applicationInfoLinksCreatorFactory));
        }
        
        /// <summary>
        /// Get the general application information.
        /// </summary>
        /// <returns>The general application information.</returns>
        /// <response code="200">Returns the general application info.</response>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApplicationInfo), 200)]
        public IActionResult GetDefault() {
            var applicationInfo = _applicationInfoProvider.Provide();
            return Ok();
        }
    }
}