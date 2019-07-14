using Dalion.Ringor.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Controllers {
    [AllowAnonymous]
    [Route("")]
    public class DefaultController : Controller {
        [HttpGet("")]
        [HttpGet("login")]
        [HttpGet("logout")]
        [HttpGet("profile")]
        [IsSpaView]
        [ReportsApplicationInfo]
        public IActionResult Index(string url) {
            return View();
        }
    }
}