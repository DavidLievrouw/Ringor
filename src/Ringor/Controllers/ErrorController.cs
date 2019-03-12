using System.Text.RegularExpressions;
using Dalion.Ringor.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Controllers {
    [AllowAnonymous]
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller {
        private static readonly Regex NotFoundRegex = new Regex("^404.*$", RegexOptions.Compiled);

        [Route("")]
        [ReportsApplicationInfo]
        [ReportsResponseType(Constants.ResponseTypes.ServerError)]
        public IActionResult InternalServerError() {
            var feature = HttpContext?.Features?.Get<IExceptionHandlerPathFeature>();
            if (!string.IsNullOrEmpty(feature?.Path)) ViewData[Constants.ViewData.ErrorPath] = feature.Path;
            if (feature?.Error != null) ViewData[Constants.ViewData.Error] = feature.Error;
            return View();
        }
        
        [Route("{*status}")]
        [ReportsApplicationInfo]
        [ReportsResponseType(Constants.ResponseTypes.CatchAllError)]
        public IActionResult CatchAllStatusCodes(string status) {
            var feature = HttpContext?.Features?.Get<IStatusCodeReExecuteFeature>();

            if (!string.IsNullOrEmpty(feature?.OriginalPathBase)) ViewData[Constants.ViewData.ErrorPathBase] = feature.OriginalPathBase;
            if (!string.IsNullOrEmpty(feature?.OriginalPath)) ViewData[Constants.ViewData.ErrorPath] = feature.OriginalPath;
            if (!string.IsNullOrEmpty(feature?.OriginalQueryString)) ViewData[Constants.ViewData.ErrorQueryString] = feature.OriginalQueryString;

            ViewData[Constants.ViewData.ErrorStatusCode] = status;

            return NotFoundRegex.IsMatch(status)
                ? View("NotFound")
                : View("OtherError");
        }
    }
}