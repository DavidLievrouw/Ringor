using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dalion.Ringor.Startup;
using Dalion.Ringor.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace Dalion.Ringor.Controllers {
    public class DefaultControllerTests {
        public class Index : DefaultControllerTests {
            public class Routing : Index, IClassFixture<WebApplicationFactory<WebHostStartup>> {
                private readonly HttpClient _client;

                public Routing() {
                    var factory = new CustomWebApplicationFactory();
                    _client = factory
                        .WithWebHostBuilder(config => { config.ConfigureServices(s => { s.AddSingleton(A.Fake<IFileProvider>()); }); })
                        .CreateClient();
                }

                private bool IsCallToSPA(HttpResponseMessage response) {
                    return response.Headers.TryGetValues("Dalion-ResponseType", out var values) && values.Contains("spa-view");
                }

                [Theory]
                [InlineData("")]
                [InlineData("/")]
                [InlineData("/login")]
                [InlineData("/login/")]
                [InlineData("/Login")]
                [InlineData("/Login/")]
                [InlineData("/login?debug=true")]
                [InlineData("/login/?debug=true")]
                [InlineData("/swaggerui")]
                [InlineData("/swaggerui/")]
                [InlineData("/Swaggerui/")]
                [InlineData("/Swaggerui")]
                [InlineData("/swaggerui?debug=true")]
                [InlineData("/swaggerui/?debug=true")]
                [InlineData("/apinav")]
                [InlineData("/apinav/")]
                [InlineData("/ApiNav/")]
                [InlineData("/ApiNav")]
                [InlineData("/apinav?debug=true")]
                [InlineData("/apinav/?debug=true")]
                [InlineData("/home")]
                [InlineData("/home/")]
                [InlineData("/home?debug=true")]
                [InlineData("/home/?debug=true")]
                public async Task MatchesWildcardRoutes(string url) {
                    var response = await _client.GetAsync(url);
                    response.Should().Match<HttpResponseMessage>(_ => IsCallToSPA(_));
                }

                [Theory]
                [InlineData("/login/page")]
                [InlineData("/login/page/segment")]
                [InlineData("/login/page?debug=true")]
                [InlineData("/swaggerui/page")]
                [InlineData("/swaggerui/page/segment")]
                [InlineData("/swaggerui/page?debug=true")]
                public async Task DoesNotMatchUrlsWithMultipleSegments(string url) {
                    var response = await _client.GetAsync(url);
                    response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
                }

                [Theory]
                [InlineData("/api")]
                [InlineData("/api/")]
                [InlineData("/api/request")]
                [InlineData("/api/r1/r2")]
                [InlineData("/api?debug=true")]
                [InlineData("/Api")]
                [InlineData("/Api/")]
                [InlineData("/Api/page")]
                [InlineData("/Api?debug=true")]
                public async Task DoesNotMatchApiUrls(string url) {
                    var response = await _client.GetAsync(url);
                    response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
                }

                [Theory]
                [InlineData("/swagger")]
                [InlineData("/swagger/")]
                [InlineData("/swagger/page")]
                [InlineData("/swagger/page/page2")]
                [InlineData("/swagger?debug=true")]
                [InlineData("/Swagger")]
                [InlineData("/Swagger/")]
                [InlineData("/Swagger/page")]
                [InlineData("/Swagger?debug=true")]
                public async Task DoesNotMatchSwaggerUrls(string url) {
                    var response = await _client.GetAsync(url);
                    response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
                }

                [Theory]
                [InlineData("/js-bundle.js")]
                [InlineData("/js-bundle.png")]
                [InlineData("/favicon.ico")]
                [InlineData("/App/js-bundle.js")]
                [InlineData("/App/assets/js-bundle.js")]
                [InlineData("/subsite/js-bundle.png")]
                [InlineData("/app/favicon.ico")]
                public async Task DoesNotMatchRouteForStaticFiles(string url) {
                    var response = await _client.GetAsync(url);
                    response.Should().Match<HttpResponseMessage>(_ => !IsCallToSPA(_));
                }
            }

            public class Logic : Index {
                private readonly string _url;
                private readonly IFileProvider _fileProvider;
                private readonly DefaultController _sut;

                public Logic() {
                    FakeFactory.Create(out _fileProvider);
                    _sut = new DefaultController(_fileProvider);
                    _url = "";
                }
                
                [Fact]
                public void WhenAllFilesExist_ReturnsViewWithExpectedModel() {
                    A.CallTo(() => _fileProvider.GetFileInfo(A<string>._))
                        .ReturnsLazily(call => new FakeFileInfo(call.GetArgument<string>(0), true));
                    var actual = _sut.Index(_url);
                    actual.Should().BeOfType<ViewResult>();
                    var expectedModel = new IndexViewModel {
                        Scripts = new[] {
                            "App/ringor-bundle.js"
                        },
                        Styles = new[] {
                            "App/ringor-bundle.css"
                        }
                    };
                    actual.As<ViewResult>().Model.Should().BeEquivalentTo(expectedModel);
                }

                [Fact]
                public void WhenSomeFilesDoNotExist_ReturnsViewWithExpectedModel() {
                    A.CallTo(() => _fileProvider.GetFileInfo(A<string>._))
                        .ReturnsLazily(call => new FakeFileInfo(
                            call.GetArgument<string>(0),
                            call.GetArgument<string>(0).StartsWith("ringor-bundle")));
                    var actual = _sut.Index(_url);
                    actual.Should().BeOfType<ViewResult>();
                    var expectedModel = new IndexViewModel {
                        Scripts = Array.Empty<string>(),
                        Styles = Array.Empty<string>()
                    };
                    actual.As<ViewResult>().Model.Should().BeEquivalentTo(expectedModel);
                }

                private class FakeFileInfo : IFileInfo {
                    public FakeFileInfo(string path, bool exists) {
                        PhysicalPath = path;
                        Exists = exists;
                    }

                    public Stream CreateReadStream() {
                        throw new NotImplementedException();
                    }

                    public bool Exists { get; }
                    public long Length { get; }
                    public string PhysicalPath { get; }
                    public string Name { get; }
                    public DateTimeOffset LastModified { get; }
                    public bool IsDirectory { get; }
                }
            }
        }
    }

    public class CustomWebApplicationFactory : WebApplicationFactory<WebHostStartup> {
        protected override IWebHostBuilder CreateWebHostBuilder() {
            var bootstrapperSettings = new BootstrapperSettings {
                EnvironmentName = EnvironmentName.Staging,
                EntryAssembly = typeof(Bootstrapper).Assembly,
                UseDetailedErrors = true
            };
            var configuration = Startup.Configuration.BuildConfiguration(bootstrapperSettings, Array.Empty<string>());
            return Bootstrapper.CreateWebHostBuilder(configuration, bootstrapperSettings);
        }
    }
}