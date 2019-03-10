using System;
using System.Collections.Generic;
using System.Linq;
using Dalion.Ringor.Api.Models;
using Dalion.Ringor.Api.Services;
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
using Xunit;

namespace Dalion.Ringor.Filters {
    public class IsSPACallFilterTests {
        private readonly IApplicationInfoProvider _applicationInfoProvider;
        private readonly IsSPACallFilterAttribute.IsSPACallFilter _sut;

        public IsSPACallFilterTests() {
            FakeFactory.Create(out _applicationInfoProvider);
            _sut = new IsSPACallFilterAttribute.IsSPACallFilter(_applicationInfoProvider);
        }

        public class OnActionExecuted : IsSPACallFilterTests {
            private readonly ApplicationInfo _applicationInfo;
            private readonly ActionExecutedContext _context;

            public OnActionExecuted() {
                _applicationInfo = new ApplicationInfo {
                    Version = "1.2.3"
                };
                A.CallTo(() => _applicationInfoProvider.Provide())
                    .Returns(_applicationInfo);
                _context = new ActionExecutedContext(
                    new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                    new List<IFilterMetadata>(),
                    null);
            }

            [Fact]
            public void WhenResultIsNotAView_DoesNothing() {
                _context.Result = new AcceptedResult();

                _sut.OnActionExecuted(_context);

                A.CallTo(() => _applicationInfoProvider.Provide()).MustNotHaveHappened();
            }

            [Fact]
            public void WhenResultIsAView_AddsApplicationInfoToViewData() {
                _context.Result = new ViewResult {
                    ViewData = new ViewDataDictionary(new FakeModelMetadataProvider(), new ModelStateDictionary())
                };

                _sut.OnActionExecuted(_context);

                var actualViewDataDic = _context.Result.As<ViewResult>().ViewData;
                actualViewDataDic.Should().ContainKey("Dalion-ApplicationInfo");
                actualViewDataDic["Dalion-ApplicationInfo"].Should().Be(_applicationInfo);
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
        }

        public class OnResultExecuting : IsSPACallFilterTests {
            private readonly ResultExecutingContext _context;

            public OnResultExecuting() {
                _context = new ResultExecutingContext(
                    new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                    Enumerable.Empty<IFilterMetadata>().ToList(),
                    A.Dummy<IActionResult>(),
                    null);
            }

            [Fact]
            public void AddsHeaderToResponse() {
                _sut.OnResultExecuting(_context);

                _context.HttpContext.Response.Headers.Should().Contain(
                    "Dalion-ResponseType",
                    new[] {"spa-view"});
            }
        }
    }
}