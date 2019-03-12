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
            if (!string.IsNullOrEmpty(feature?.Path)) ViewData[Constants.ViewData.ErrorPath] = feature.Path;
            if (feature?.Error != null) ViewData[Constants.ViewData.Error] = feature.Error;
            return View();
        }

        [Route("404")]
        [ReportsApplicationInfo]
        public IActionResult NotFound404() {
            var feature = HttpContext?.Features?.Get<IStatusCodeReExecuteFeature>();
            if (!string.IsNullOrEmpty(feature?.OriginalPathBase)) ViewData[Constants.ViewData.ErrorPathBase] = feature?.OriginalPathBase;
            if (!string.IsNullOrEmpty(feature?.OriginalPath)) ViewData[Constants.ViewData.ErrorPath] = feature?.OriginalPath;
            if (!string.IsNullOrEmpty(feature?.OriginalQueryString)) ViewData[Constants.ViewData.ErrorQueryString] = feature?.OriginalQueryString;
            return View();
        }

        [Route("{*url:regex(^((?!/404).)*$)}")]
        [ReportsApplicationInfo]
        public IActionResult OtherError() {
            return View();
        }
    }
}