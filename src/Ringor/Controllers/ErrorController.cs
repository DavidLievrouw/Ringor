using System.Linq;
using System.Text.RegularExpressions;
using Dalion.Ringor.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Controllers {
    [AllowAnonymous]
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller {
        private static readonly Regex ApiCallRegex = new Regex(@"^/?api.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex StatusCodeRegex = new Regex(@"\d+", RegexOptions.Compiled);

        [Route("")]
        [ReportsApplicationInfo]
        [ReportsResponseType(Constants.ResponseTypes.ServerError)]
        public IActionResult InternalServerError() {
            var feature = HttpContext.Features?.Get<IExceptionHandlerPathFeature>();
            if (!string.IsNullOrEmpty(feature?.Path)) ViewData[Constants.ViewData.ErrorPath] = feature.Path;
            if (feature?.Error != null) ViewData[Constants.ViewData.Error] = feature.Error;
            return View();
        }

        [Route("{*status}")]
        [ReportsApplicationInfo]
        [ReportsResponseType(Constants.ResponseTypes.CatchAllError)]
        public IActionResult CatchAllStatusCodes(string status, [FromQuery] string path) {
            var feature = HttpContext.Features?.Get<IStatusCodeReExecuteFeature>();

            if (!string.IsNullOrEmpty(feature?.OriginalPathBase)) ViewData[Constants.ViewData.ErrorPathBase] = feature.OriginalPathBase;
            if (!string.IsNullOrEmpty(feature?.OriginalPath)) ViewData[Constants.ViewData.ErrorPath] = feature.OriginalPath;
            if (!string.IsNullOrEmpty(feature?.OriginalQueryString)) ViewData[Constants.ViewData.ErrorQueryString] = feature.OriginalQueryString;
            if (!ViewData.ContainsKey(Constants.ViewData.ErrorPath) && !string.IsNullOrEmpty(path)) ViewData[Constants.ViewData.ErrorPath] = path;

            var statusCodeString = StatusCodeRegex.Matches(status).FirstOrDefault()?.Value;
            if (!int.TryParse(statusCodeString, out var statusCode)) {
                statusCode = HttpContext.Response.StatusCode;
            }
            ViewData[Constants.ViewData.ErrorStatusCode] = statusCode;

            var isApiCall = !string.IsNullOrEmpty(feature?.OriginalPath) && ApiCallRegex.IsMatch(feature.OriginalPath);

            if (isApiCall) {
                return StatusCode(statusCode);
            }

            var view = statusCode == StatusCodes.Status404NotFound
                ? View("NotFound")
                : View("OtherError");

            view.StatusCode = statusCode;

            return view;
        }
    }
}