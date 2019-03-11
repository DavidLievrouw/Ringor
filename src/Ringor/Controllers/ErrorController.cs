using Dalion.Ringor.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Controllers {
    [AllowAnonymous]
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller {
        [Route("")]
        [ReportsApplicationInfo]
        public IActionResult InternalServerError() {
            var exceptionHandlerPathFeature = HttpContext?.Features?.Get<IExceptionHandlerPathFeature>();
            var path = exceptionHandlerPathFeature?.Path;
            var exception = exceptionHandlerPathFeature?.Error;
            return View();
        }

        [Route("404")]
        [ReportsApplicationInfo]
        public IActionResult NotFound404() {
            return View();
        }
    }
}