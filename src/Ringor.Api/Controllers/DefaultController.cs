using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Models.Links;
using Dalion.Ringor.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Api.Controllers {
    /// <summary>
    /// The entry point of the Api.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api")]
    [DisplayName("Api home")]
    [AllowAnonymous]
    public class DefaultController : Controller {
        private readonly IApplicationInfoProvider _applicationInfoProvider;
        private readonly IApiHomeResponseLinksCreatorFactory _apiHomeResponseLinksCreatorFactory;

        public DefaultController(IApplicationInfoProvider applicationInfoProvider, IApiHomeResponseLinksCreatorFactory apiHomeResponseLinksCreatorFactory) {
            _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
            _apiHomeResponseLinksCreatorFactory = apiHomeResponseLinksCreatorFactory ?? throw new ArgumentNullException(nameof(apiHomeResponseLinksCreatorFactory));
        }
        
        /// <summary>
        /// Get the API root, including the general application information.
        /// </summary>
        /// <returns>The API root, including the general application information.</returns>
        /// <response code="200">Returns the API root, including the general application information.</response>
        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiHomeResponse), 200)]
        public async Task<IActionResult> GetDefault() {
            var response = new ApiHomeResponse {
                ApplicationInfo = _applicationInfoProvider.Provide()
            };
            await _apiHomeResponseLinksCreatorFactory.Create().CreateLinksFor(response);
            return Ok(response);
        }
    }
}