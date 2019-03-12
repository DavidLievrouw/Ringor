using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Dalion.Ringor.Filters {
    public class ReportsResponseTypeAttributeTests {
        private readonly string _responseType;
        private readonly ReportsResponseTypeAttribute.ReportsResponseTypeFilter _sut;

        public ReportsResponseTypeAttributeTests() {
            _responseType = "UnitTestResponseType";
            _sut = new ReportsResponseTypeAttribute.ReportsResponseTypeFilter(_responseType);
        }

        public class OnResultExecuting : ReportsResponseTypeAttributeTests {
            private readonly ResultExecutingContext _context;

            public OnResultExecuting() {
                _context = new ResultExecutingContext(
                    new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                    Enumerable.Empty<IFilterMetadata>().ToList(),
                    new ViewResult(),
                    null);
            }

            [Fact]
            public void IfResultIsViewResult_AddsHeaderToResponse() {
                _context.Result = new ViewResult();

                _sut.OnResultExecuting(_context);

                _context.HttpContext.Response.Headers.Should().Contain(
                    "Dalion-ResponseType",
                    new[] {_responseType});
            }

            [Fact]
            public void IfResultIsNotViewResult_DoesNotAddHeaderToResponse() {
                _context.Result = new NotFoundResult();

                _sut.OnResultExecuting(_context);

                _context.HttpContext.Response.Headers.Should().NotContainKey("Dalion-ResponseType");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void WhenResponseTypeIsNullOrEmpty_DoesNotThrow_AddsHeaderWithNullValue(string noResponseType) {
                var sut = new ReportsResponseTypeAttribute.ReportsResponseTypeFilter(noResponseType);
                _context.Result = new ViewResult();

                sut.OnResultExecuting(_context);

                _context.HttpContext.Response.Headers.Should().Contain(
                    "Dalion-ResponseType",
                    new string[0]);
            }
        }
    }
}