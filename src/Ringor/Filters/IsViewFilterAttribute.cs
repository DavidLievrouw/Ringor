using System;
using System.Linq;
using Dalion.Ringor.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Dalion.Ringor.Filters {
    public class IsViewFilterAttribute : Attribute, IFilterFactory {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) {
            return new IsViewFilter(
                serviceProvider.GetRequiredService<IApplicationInfoProvider>(),
                serviceProvider.GetRequiredService<IFileProvider>());
        }

        public bool IsReusable => false;

        internal class IsViewFilter : ActionFilterAttribute {
            private readonly IApplicationInfoProvider _applicationInfoProvider;
            private readonly IFileProvider _fileProvider;

            public IsViewFilter(IApplicationInfoProvider applicationInfoProvider, IFileProvider fileProvider) {
                _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
                _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            }

            public override void OnActionExecuted(ActionExecutedContext context) {
                base.OnActionExecuted(context);

                if (context.Result is ViewResult viewResult) {
                    viewResult.ViewData[Constants.ViewData.ApplicationInfo] = _applicationInfoProvider.Provide();
                    viewResult.ViewData[Constants.ViewData.Scripts] = new[] {"App/ringor-bundle.js"}
                        .Where(relativePath => _fileProvider.GetFileInfo(relativePath).Exists)
                        .ToList();
                    viewResult.ViewData[Constants.ViewData.Styles] = new[] {"App/ringor-bundle.css"}
                        .Where(relativePath => _fileProvider.GetFileInfo(relativePath).Exists)
                        .ToList();
                }
            }

            public override void OnResultExecuting(ResultExecutingContext context) {
                base.OnResultExecuting(context);

                if (context.Result is ViewResult) {
                    context.HttpContext.Response.Headers.Add(
                        Constants.Headers.ResponseType,
                        new[] {Constants.ResponseTypes.View});
                }
            }
        }
    }
}