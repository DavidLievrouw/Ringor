using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Api.Controllers {
    [Route("api")]
    public class DefaultController : Controller {
        [HttpGet("")]
        public IActionResult GetDefault() {
            return Ok(new {
                Info = "The 'Ringor' API is running."
            });
        }
    }
}