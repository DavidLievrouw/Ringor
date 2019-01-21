using System;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Dalion.Ringor.Api.Models.Links {
    public class ApplicationUriResolverTests {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationUriResolver _sut;

        public ApplicationUriResolverTests() {
            FakeFactory.Create(out _httpContextAccessor);
            _sut = new ApplicationUriResolver(_httpContextAccessor);
        }

        public class Resolve : ApplicationUriResolverTests {
            private readonly DefaultHttpContext _httpContext;

            public Resolve() {
                _httpContext = new DefaultHttpContext();
                _httpContext.Request.Method = HttpMethods.Post;
                _httpContext.Request.Host = new HostString("tests.recomatics.com", 8080);
                _httpContext.Request.IsHttps = true;
                _httpContext.Request.PathBase = new PathString("/api");
                _httpContext.Request.Path = new PathString("/documents/doc_001");
                A.CallTo(() => _httpContextAccessor.HttpContext).Returns(_httpContext);
            }

            [Fact]
            public void ReturnsExpectedApplicationUrl() {
                var actual = _sut.Resolve();
                var expectedUri = new Uri("https://tests.recomatics.com:8080/api/", UriKind.Absolute);
                actual.Should().BeEquivalentTo(expectedUri);
            }

            [Fact]
            public void GivenNoPathBase_ReturnsExpectedApplicationUrl() {
                _httpContext.Request.PathBase = new PathString("");
                _httpContext.Request.Path = new PathString("/api/documents/doc_001");

                var actual = _sut.Resolve();

                var expectedUri = new Uri("https://tests.recomatics.com:8080/", UriKind.Absolute);
                actual.Should().BeEquivalentTo(expectedUri);
            }
        }
    }
}