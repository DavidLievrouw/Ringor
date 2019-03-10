using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Dalion.Ringor.Constraints {
    public class GetSpaActionConstraintAttribute : Attribute, IActionConstraint {
        private static readonly Regex SpaPathRegex = new Regex(@"^$|^/*((?!(/(?!\?).)+|(api(?!nav))|(swagger(?!ui))|.*\.+.+).)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public bool Accept(ActionConstraintContext context) {
            var request = context.RouteContext.HttpContext.Request;

            if (request.Method != HttpMethods.Get) return false;

            var path = request.Path;
            var isRegexMatch = SpaPathRegex.IsMatch(path);

            return isRegexMatch;
        }

        public int Order => -10;
    }
}