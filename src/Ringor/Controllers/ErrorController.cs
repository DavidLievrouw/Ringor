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
        [IsViewFilter]
        public IActionResult InternalServerError() {
            var exceptionHandlerPathFeature = HttpContext?.Features?.Get<IExceptionHandlerPathFeature>();
            var path = exceptionHandlerPathFeature?.Path;
            var exception = exceptionHandlerPathFeature?.Error;
            return View();
        }

        [Route("404")]
        [IsViewFilter]
        public IActionResult NotFound404() {
            return View();
        }
    }
}