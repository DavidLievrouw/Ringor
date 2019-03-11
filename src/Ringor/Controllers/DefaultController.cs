using Dalion.Ringor.Constraints;
using Dalion.Ringor.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.Ringor.Controllers {
    [AllowAnonymous]
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DefaultController : Controller {
        [HttpGet("{*url}")]
        [GetSpaActionConstraint]
        [IsViewFilter]
        public IActionResult Index(string url) {
            return View();
        }
    }
}