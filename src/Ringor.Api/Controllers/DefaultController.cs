using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Api.Controllers {
    [Route("api")]
    [Authorize]
    public class DefaultController : Controller {
        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult GetDefault() {
            return Ok(new {
                Info = "The 'Ringor' API is running."
            });
        }
    }
}