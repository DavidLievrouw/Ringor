using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dalion.Ringor.Filters {
    public class ReportsResponseTypeAttribute : Attribute, IFilterFactory {
        private readonly string _responseType;

        public ReportsResponseTypeAttribute(string responseType) {
            _responseType = responseType;
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) {
            return new ReportsResponseTypeFilter(_responseType);
        }

        public bool IsReusable => false;

        internal class ReportsResponseTypeFilter : ActionFilterAttribute {
            private readonly string _responseType;

            public ReportsResponseTypeFilter(string responseType) {
                _responseType = responseType;
            }

            public override void OnResultExecuting(ResultExecutingContext context) {
                base.OnResultExecuting(context);

                if (context.Result is ViewResult) {
                    context.HttpContext.Response.Headers.Add(
                        Constants.Headers.ResponseType,
                        string.IsNullOrEmpty(_responseType)
                            ? Array.Empty<string>()
                            : new[] {_responseType});
                }
            }
        }
    }
}