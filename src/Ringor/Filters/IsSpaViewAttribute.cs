using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Dalion.Ringor.Filters {
    public class IsSpaViewAttribute : Attribute, IFilterFactory {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) {
            return new IsSpaViewFilter(serviceProvider.GetRequiredService<IFileProvider>());
        }

        public bool IsReusable => false;

        internal class IsSpaViewFilter : ActionFilterAttribute {
            private readonly IFileProvider _fileProvider;

            public IsSpaViewFilter(IFileProvider fileProvider) {
                _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            }

            public override void OnActionExecuted(ActionExecutedContext context) {
                base.OnActionExecuted(context);

                if (context.Result is ViewResult viewResult) {
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
                        new[] {Constants.ResponseTypes.SpaView});
                }
            }
        }
    }
}