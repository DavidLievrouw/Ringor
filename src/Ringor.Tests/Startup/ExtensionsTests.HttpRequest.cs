using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Dalion.Ringor.Startup {
    public partial class ExtensionsTests {
        public class HttpRequest {
            public class GetRawSecurityToken : HttpRequest {
                private readonly Microsoft.AspNetCore.Http.HttpRequest _request;

                public GetRawSecurityToken() {
                    _request = new DefaultHttpRequest(new DefaultHttpContext()) {
                        Method = HttpMethods.Get,
                        Scheme = "https",
                        Host = new HostString("localhost", 443),
                        Path = new PathString("/api")
                    };
                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void GivenNullRequest_ThrowsArgumentNullException() {
                    Microsoft.AspNetCore.Http.HttpRequest nullRequest = null;
                    Action act = () => nullRequest.GetRawSecurityToken();
                    act.Should().Throw<ArgumentNullException>();
                }

                [Fact]
                public void WhenAuthorizationHeaderIsMissing_ReturnsNull() {
                    _request.Headers.Clear();
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().BeNull();
                }

                [Fact]
                public void WhenAuthorizationHeaderIsNull_ReturnsNull() {
                    _request.Headers["Authorization"] = new StringValues(new string[] {null});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().BeNull();
                }

                [Fact]
                public void WhenAuthorizationHeaderIsEmpty_ReturnsNull() {
                    _request.Headers["Authorization"] = new StringValues(new[] {""});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().BeNull();
                }

                [Fact]
                public void WhenAuthorizationHeaderIsIncorrectlyCased_ReturnsBearerTokenValue() {
                    _request.Headers["authorization"] = new StringValues(new[] {"Bearer eyabc123=="});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().Be("eyabc123==");
                }

                [Fact]
                public void WhenAuthorizationHeaderDoesNotContainABearerToken_ReturnsNull() {
                    _request.Headers["authorization"] = new StringValues(new[] {"Digest username=\"Mufasa\""});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().BeNull();
                }

                [Fact]
                public void WhenBearerPrefixIsIncorrectlyCased_ReturnsBearerTokenValue() {
                    _request.Headers["Authorization"] = new StringValues(new[] {"bearer eyabc123=="});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().Be("eyabc123==");
                }

                [Fact]
                public void WhenAuthorizationHeaderHasMultipleValues_ReturnsBearerTokenFromFirstValue() {
                    _request.Headers["Authorization"] = new StringValues(new[] {"Bearer eyabc123==", "Bearer eyabc456=="});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().Be("eyabc123==");
                }

                [Fact]
                public void WhenAuthorizationHeaderHasMultipleValues_ButOnlyOneBearerToken_ReturnsBearerToken() {
                    _request.Headers["Authorization"] = new StringValues(new[] {"Digest username=\"Mufasa\"", "Bearer eyabc456=="});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().Be("eyabc456==");
                }

                [Fact]
                public void ReturnsBearerTokenValue() {
                    _request.Headers["Authorization"] = new StringValues(new[] {"Bearer eyabc123=="});
                    var actual = _request.GetRawSecurityToken();
                    actual.Should().Be("eyabc123==");
                }
            }
        }
    }
}