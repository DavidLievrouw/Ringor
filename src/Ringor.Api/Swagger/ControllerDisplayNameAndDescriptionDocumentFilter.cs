using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dalion.Ringor.Api.Swagger {
    internal class ControllerDisplayNameAndDescriptionDocumentFilter : IDocumentFilter {
        private readonly XPathNavigator _xmlNavigator;

        public ControllerDisplayNameAndDescriptionDocumentFilter(IXPathNavigable xmlDoc) {
            _xmlNavigator = xmlDoc.CreateNavigator();
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context) {
            foreach (var keyValuePair in context.ApiDescriptions
                .Select(apiDesc => apiDesc.ActionDescriptor as ControllerActionDescriptor)
                .SkipWhile(actionDesc => actionDesc == null)
                .GroupBy(actionDesc => {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var attr = actionDesc
                        .MethodInfo
                        .DeclaringType
                        .GetCustomAttribute<DisplayNameAttribute>();
                    return attr?.DisplayName ?? actionDesc.ControllerName;
                })
                .ToDictionary(grp => grp.Key, grp => grp.Last().ControllerTypeInfo.AsType())) {
                var xpathNavigator1 = _xmlNavigator.SelectSingleNode($"/doc/members/member[@name='{XmlCommentsMemberNameHelper.GetMemberNameForType(keyValuePair.Value)}']");
                var xpathNavigator2 = xpathNavigator1?.SelectSingleNode("summary");
                if (xpathNavigator2 != null) {
                    if (swaggerDoc.Tags == null) {
                        swaggerDoc.Tags = new List<Tag>();
                    }
                    swaggerDoc.Tags.Add(new Tag() {
                        Name = keyValuePair.Key,
                        Description = XmlCommentsTextHelper.Humanize(xpathNavigator2.InnerXml)
                    });
                }
            }
        }
    }
}