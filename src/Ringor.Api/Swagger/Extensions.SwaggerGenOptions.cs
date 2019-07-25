using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml.XPath;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dalion.Ringor.Api.Swagger {
    public static partial class Extensions {
        public static void IncludeControllerDisplayNameAndDescription(this SwaggerGenOptions swaggerGenOptions, string filePath) {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

            var xpathDocument = new XPathDocument(filePath);
            swaggerGenOptions.DocumentFilter<ControllerDisplayNameAndDescriptionDocumentFilter>(xpathDocument);

            swaggerGenOptions.TagActionsBy(apiDesc => {
                var actionDesc = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                var attr = actionDesc
                    ?.MethodInfo
                    ?.DeclaringType
                    ?.GetCustomAttribute<DisplayNameAttribute>();
                return new[] {attr?.DisplayName ?? actionDesc?.ControllerName ?? apiDesc.ActionDescriptor.RouteValues["controller"]};
            });
        }
    }
}