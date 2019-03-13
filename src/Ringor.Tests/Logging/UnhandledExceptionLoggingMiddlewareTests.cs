using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Serilog;
using Xunit;

namespace Dalion.Ringor.Logging {
    public class UnhandledExceptionLoggingMiddlewareTests {
        private readonly ILogger _logger;

        public UnhandledExceptionLoggingMiddlewareTests() {
            FakeFactory.Create(out _logger);
        }

        private UnhandledExceptionLoggingMiddleware CreateSut(RequestDelegate next) {
            return new UnhandledExceptionLoggingMiddleware(next, _logger);
        }

        public class Invoke : UnhandledExceptionLoggingMiddlewareTests {
            private readonly HttpContext _context;

            public Invoke() {
                _context = new DefaultHttpContext();
            }

            [Fact]
            public void WhenNextMiddlewareDoesNotThrow_DoesNotLog() {
                HttpContext nextInvokedWith = null;
                var sut = CreateSut(context => {
                    nextInvokedWith = context;
                    return Task.CompletedTask;
                });
                Func<Task> act = () => sut.Invoke(_context);
                act.Should().NotThrow();
                nextInvokedWith.Should().Be(_context);
            }

            [Fact]
            public void WhenNextMiddlewareThrows_LogsError() {
                var failure = new InvalidOperationException("Epic fail");
                var sut = CreateSut(context => throw failure);
                Func<Task> act = () => sut.Invoke(_context);
                act.Should().Throw<InvalidOperationException>();
                A.CallTo(() => _logger.Fatal(failure, A<string>._)).MustHaveHappened();
            }

            [Fact]
            public void WhenNextMiddlewareThrows_Rethrows() {
                var failure = new InvalidOperationException("Epic fail");
                var sut = CreateSut(context => throw failure);
                Func<Task> act = () => sut.Invoke(_context);
                act.Should().Throw<InvalidOperationException>().Where(ex => ex.Message == failure.Message);
            }
        }
    }
}