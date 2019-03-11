using System;
using Dalion.Ringor.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Dalion.Ringor.Filters {
    public class ReportsApplicationInfoAttribute : Attribute, IFilterFactory {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) {
            return new ReportsApplicationInfoFilter(serviceProvider.GetRequiredService<IApplicationInfoProvider>());
        }

        public bool IsReusable => false;

        internal class ReportsApplicationInfoFilter : ActionFilterAttribute {
            private readonly IApplicationInfoProvider _applicationInfoProvider;

            public ReportsApplicationInfoFilter(IApplicationInfoProvider applicationInfoProvider) {
                _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
            }

            public override void OnActionExecuted(ActionExecutedContext context) {
                base.OnActionExecuted(context);

                if (context.Result is ViewResult viewResult) {
                    viewResult.ViewData[Constants.ViewData.ApplicationInfo] = _applicationInfoProvider.Provide();
                }
            }
        }
    }
}