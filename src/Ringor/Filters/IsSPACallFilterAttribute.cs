using System;
using Dalion.Ringor.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Filters {
    public class IsSPACallFilterAttribute : Attribute, IFilterFactory {
        internal class IsSPACallFilter : ActionFilterAttribute {
            private readonly IApplicationInfoProvider _applicationInfoProvider;

            public IsSPACallFilter(IApplicationInfoProvider applicationInfoProvider) {
                _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
            }

            public override void OnActionExecuted(ActionExecutedContext context) {
                base.OnActionExecuted(context);

                if (context.Result is ViewResult viewResult) {
                    viewResult.ViewData[Constants.ViewData.ApplicationInfo] = _applicationInfoProvider.Provide();
                }
            }

            public override void OnResultExecuting(ResultExecutingContext context) {
                base.OnResultExecuting(context);
                
                context.HttpContext.Response.Headers.Add(
                    Constants.Headers.ResponseType, 
                    new [] {"spa-view"});
            }
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) {
            return new IsSPACallFilter(serviceProvider.GetRequiredService<IApplicationInfoProvider>());
        }

        public bool IsReusable => false;
    }
}