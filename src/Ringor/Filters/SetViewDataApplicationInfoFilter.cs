using System;
using Dalion.Ringor.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dalion.Ringor.Filters {
    public class SetViewDataApplicationInfoFilter : ActionFilterAttribute {
        private readonly IApplicationInfoProvider _applicationInfoProvider;

        public SetViewDataApplicationInfoFilter(IApplicationInfoProvider applicationInfoProvider) {
            _applicationInfoProvider = applicationInfoProvider ?? throw new ArgumentNullException(nameof(applicationInfoProvider));
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            base.OnActionExecuted(context);

            if (context.Result is ViewResult viewResult) {
                viewResult.ViewData["Dalion-ApplicationInfo"] = _applicationInfoProvider.Provide();
            }
        }
    }
}