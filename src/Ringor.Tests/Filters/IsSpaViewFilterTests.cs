using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace Dalion.Ringor.Filters {
    public class IsSpaViewFilterTests {
        private readonly IFileProvider _fileProvider;
        private readonly IsSpaViewAttribute.IsSpaViewFilter _sut;

        public IsSpaViewFilterTests() {
            FakeFactory.Create(out _fileProvider);
            _sut = new IsSpaViewAttribute.IsSpaViewFilter(_fileProvider);
        }

        public class OnActionExecuted : IsSpaViewFilterTests {
            private readonly ActionExecutedContext _context;

            public OnActionExecuted() {
                _context = new ActionExecutedContext(
                    new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                    new List<IFilterMetadata>(),
                    null);
            }

            [Fact]
            public void WhenResultIsNotAView_DoesNothing() {
                _context.Result = new AcceptedResult();

                _sut.OnActionExecuted(_context);

                var actualViewDataDic = _context.Result.As<ViewResult>()?.ViewData;
                actualViewDataDic.Should().Match<ViewDataDictionary>(dic => dic == null || dic.Count < 1);
            }

            [Fact]
            public void WhenResultIsAView_AddsScriptsToViewData() {
                _context.Result = new ViewResult {
                    ViewData = new ViewDataDictionary(new FakeModelMetadataProvider(), new ModelStateDictionary())
                };
                A.CallTo(() => _fileProvider.GetFileInfo(A<string>._))
                    .ReturnsLazily(call => new FakeFileInfo(call.GetArgument<string>(0), true));

                _sut.OnActionExecuted(_context);

                var actualViewDataDic = _context.Result.As<ViewResult>().ViewData;
                actualViewDataDic.Should().ContainKey("Dalion-Scripts");
                actualViewDataDic["Dalion-Scripts"].Should().BeEquivalentTo(new[] {
                    "App/ringor-bundle.js"
                });
            }

            [Fact]
            public void WhenResultIsAView_AddsOnlyExistingScriptsToViewData() {
                _context.Result = new ViewResult {
                    ViewData = new ViewDataDictionary(new FakeModelMetadataProvider(), new ModelStateDictionary())
                };
                A.CallTo(() => _fileProvider.GetFileInfo(A<string>._))
                    .ReturnsLazily(call => new FakeFileInfo(
                        call.GetArgument<string>(0),
                        call.GetArgument<string>(0).StartsWith("ringor-bundle")));

                _sut.OnActionExecuted(_context);

                var actualViewDataDic = _context.Result.As<ViewResult>().ViewData;
                actualViewDataDic.Should().ContainKey("Dalion-Scripts");
                actualViewDataDic["Dalion-Scripts"].Should().BeEquivalentTo(Array.Empty<string>());
            }

            [Fact]
            public void WhenResultIsAView_AddsStylesToViewData() {
                _context.Result = new ViewResult {
                    ViewData = new ViewDataDictionary(new FakeModelMetadataProvider(), new ModelStateDictionary())
                };
                A.CallTo(() => _fileProvider.GetFileInfo(A<string>._))
                    .ReturnsLazily(call => new FakeFileInfo(call.GetArgument<string>(0), true));

                _sut.OnActionExecuted(_context);

                var actualViewDataDic = _context.Result.As<ViewResult>().ViewData;
                actualViewDataDic.Should().ContainKey("Dalion-Styles");
                actualViewDataDic["Dalion-Styles"].Should().BeEquivalentTo(new[] {
                    "App/ringor-bundle.css"
                });
            }

            [Fact]
            public void WhenResultIsAView_AddsOnlyExistingStylesToViewData() {
                _context.Result = new ViewResult {
                    ViewData = new ViewDataDictionary(new FakeModelMetadataProvider(), new ModelStateDictionary())
                };
                A.CallTo(() => _fileProvider.GetFileInfo(A<string>._))
                    .ReturnsLazily(call => new FakeFileInfo(
                        call.GetArgument<string>(0),
                        call.GetArgument<string>(0).StartsWith("ringor-bundle")));

                _sut.OnActionExecuted(_context);

                var actualViewDataDic = _context.Result.As<ViewResult>().ViewData;
                actualViewDataDic.Should().ContainKey("Dalion-Styles");
                actualViewDataDic["Dalion-Styles"].Should().BeEquivalentTo(Array.Empty<string>());
                ;
            }

            private class FakeModelMetadataProvider : IModelMetadataProvider {
                public ModelMetadata GetMetadataForType(Type modelType) {
                    return new FakeModelMetadata(ModelMetadataIdentity.ForType(typeof(string)));
                }

                public IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType) {
                    throw new NotImplementedException();
                }
            }

            private class FakeModelMetadata : ModelMetadata {
                public FakeModelMetadata(ModelMetadataIdentity identity) : base(identity) { }
                public override IReadOnlyDictionary<object, object> AdditionalValues { get; }
                public override ModelPropertyCollection Properties { get; }
                public override string BinderModelName { get; }
                public override Type BinderType { get; }
                public override BindingSource BindingSource { get; }
                public override bool ConvertEmptyStringToNull { get; }
                public override string DataTypeName { get; }
                public override string Description { get; }
                public override string DisplayFormatString { get; }
                public override string DisplayName { get; }
                public override string EditFormatString { get; }
                public override ModelMetadata ElementMetadata { get; }
                public override IEnumerable<KeyValuePair<EnumGroupAndName, string>> EnumGroupedDisplayNamesAndValues { get; }
                public override IReadOnlyDictionary<string, string> EnumNamesAndValues { get; }
                public override bool HasNonDefaultEditFormat { get; }
                public override bool HtmlEncode { get; }
                public override bool HideSurroundingHtml { get; }
                public override bool IsBindingAllowed { get; }
                public override bool IsBindingRequired { get; }
                public override bool IsEnum { get; }
                public override bool IsFlagsEnum { get; }
                public override bool IsReadOnly { get; }
                public override bool IsRequired { get; }
                public override ModelBindingMessageProvider ModelBindingMessageProvider { get; }
                public override int Order { get; }
                public override string Placeholder { get; }
                public override string NullDisplayText { get; }
                public override IPropertyFilterProvider PropertyFilterProvider { get; }
                public override bool ShowForDisplay { get; }
                public override bool ShowForEdit { get; }
                public override string SimpleDisplayProperty { get; }
                public override string TemplateHint { get; }
                public override bool ValidateChildren { get; }
                public override IReadOnlyList<object> ValidatorMetadata { get; }
                public override Func<object, object> PropertyGetter { get; }
                public override Action<object, object> PropertySetter { get; }
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

        public class OnResultExecuting : IsSpaViewFilterTests {
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
                    new[] {"SPAView"});
            }

            [Fact]
            public void IfResultIsNotViewResult_DoesNotAddHeaderToResponse() {
                _context.Result = new NotFoundResult();

                _sut.OnResultExecuting(_context);

                _context.HttpContext.Response.Headers.Should().NotContainKey("Dalion-ResponseType");
            }
        }
    }
}