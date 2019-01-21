using System;
using System.IO;
using Dalion.Ringor.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace Dalion.Ringor.Controllers {
    public class DefaultControllerTests {
        private readonly IFileProvider _fileProvider;
        private readonly DefaultController _sut;

        public DefaultControllerTests() {
            FakeFactory.Create(out _fileProvider);
            _sut = new DefaultController(_fileProvider);
        }

        public class Index : DefaultControllerTests {
            [Fact]
            public void WhenAllFilesExist_ReturnsViewWithExpectedModel() {
                A.CallTo(() => _fileProvider.GetFileInfo(A<string>._))
                    .ReturnsLazily(call => new FakeFileInfo(call.GetArgument<string>(0), true));
                var actual = _sut.Index();
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
                var actual = _sut.Index();
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