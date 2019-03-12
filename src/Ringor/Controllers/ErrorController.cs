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
            var feature = HttpContext?.Features?.Get<IExceptionHandlerPathFeature>();
            ViewData[Constants.ViewData.ErrorPath] = feature?.Path;
            ViewData[Constants.ViewData.Error] = feature?.Error;
            return View();
        }

        [Route("404")]
        [ReportsApplicationInfo]
        public IActionResult NotFound404() {
            var feature = HttpContext?.Features?.Get<IStatusCodeReExecuteFeature>();
            ViewData[Constants.ViewData.ErrorPath] = feature?.OriginalPath;
            return View();
        }

        [Route("{*url:regex(^((?!/404).)*$)}")]
        [ReportsApplicationInfo]
        public IActionResult OtherError() {
            return View();
        }
    }
}