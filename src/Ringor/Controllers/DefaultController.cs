using System;
using System.Linq;
using Dalion.Ringor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Dalion.Ringor.Controllers {
    [AllowAnonymous]
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DefaultController : Controller {
        private readonly IFileProvider _fileProvider;

        public DefaultController(IFileProvider fileProvider) {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        }

        [HttpGet("")]
        public IActionResult Index() {
            var indexViewModel = new IndexViewModel {
                Scripts = new[] {"App/ringor-bundle.js"}
                    .Where(relativePath => _fileProvider.GetFileInfo(relativePath).Exists),
                Styles = new[] {"App/ringor-bundle.css"}
                    .Where(relativePath => _fileProvider.GetFileInfo(relativePath).Exists)
            };
            return View(indexViewModel);
        }
    }
}