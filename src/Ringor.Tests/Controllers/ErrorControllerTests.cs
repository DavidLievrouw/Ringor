using System.Net.Http;
using System.Threading.Tasks;
using Dalion.Ringor.Startup;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Dalion.Ringor.Controllers {
    public class ErrorControllerTests {
        private readonly ErrorController _sut;

        public ErrorControllerTests() {
            _sut = new ErrorController();
        }

        public class InternalServerError : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;

            public InternalServerError() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.InternalServerError();
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }
        }

        public class NotFound404 : ErrorControllerTests, IClassFixture<WebApplicationFactory<WebHostStartup>> {
            private readonly HttpClient _client;

            public NotFound404() {
                var factory = new CustomWebApplicationFactory();
                _client = factory.CreateClient();
            }

            [Fact]
            public void ReturnsView() {
                var actual = _sut.NotFound404();
                actual.Should().NotBeNull().And.BeAssignableTo<ViewResult>();
            }
        }
    }
}