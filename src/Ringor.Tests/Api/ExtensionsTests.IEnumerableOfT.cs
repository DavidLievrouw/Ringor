using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Api {
    public partial class ExtensionsTests {
        public class EnumerableOfT : ExtensionsTests {
            public class ForEach : EnumerableOfT {
                public ForEach() {
                    SourceItem.ResetPerformedActionCount();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Action act = () => source.ForEach(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.ForEach((Action<SourceItem>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionPerformed.Should().BeFalse();
                    }
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Action act = () => source.ForEach(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenSourceWithElements_PerformsActionForAllElements() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.ForEach(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionPerformed.Should().BeTrue();
                    }
                }

                private class SourceItem {
                    public static int PerformedActionCount { get; private set; }

                    public bool ActionPerformed { get; private set; }

                    public static void ResetPerformedActionCount() {
                        PerformedActionCount = 0;
                    }

                    public void DoSomeAction() {
                        ActionPerformed = true;
                        PerformedActionCount++;
                    }
                }
            }

            public class ForEachWithIndex : EnumerableOfT {
                public ForEachWithIndex() {
                    SourceItem.ResetPerformedActions();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Action act = () => source.ForEach((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.ForEach((Action<SourceItem, int>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(-1);
                    }
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Action act = () => source.ForEach((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenSourceWithElements_PerformsActionForAllElements_WithCorrectIndex() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.ForEach((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Count.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(Array.IndexOf(source, sourceItem));
                    }
                }

                private class SourceItem {
                    public static Dictionary<SourceItem, int> PerformedActions;

                    public SourceItem() {
                        ActionIndex = -1;
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public int ActionIndex { get; private set; }

                    public static void ResetPerformedActions() {
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public void DoSomeAction(int index) {
                        ActionIndex = index;
                        PerformedActions[this] = index;
                    }
                }
            }
            
            public class ForEachAsync : EnumerableOfT {
                public ForEachAsync() {
                    SourceItem.ResetPerformedActionCount();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Func<Task> act = () => source.ForEachAsync(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEachAsync((Func<SourceItem, Task>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionPerformed.Should().BeFalse();
                    }
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Func<Task> act = () => source.ForEachAsync(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenSourceWithElements_PerformsActionForAllElements() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEachAsync(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionPerformed.Should().BeTrue();
                    }
                }

                private class SourceItem {
                    public static int PerformedActionCount { get; private set; }

                    public bool ActionPerformed { get; private set; }

                    public static void ResetPerformedActionCount() {
                        PerformedActionCount = 0;
                    }

                    public Task DoSomeAction() {
                        ActionPerformed = true;
                        PerformedActionCount++;
                        return Task.CompletedTask;
                    }
                }
            }

            public class ForEachAsyncWithIndex : EnumerableOfT {
                public ForEachAsyncWithIndex() {
                    SourceItem.ResetPerformedActions();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Func<Task> act = () => source.ForEachAsync((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEachAsync((Func<SourceItem, int, Task>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(-1);
                    }
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Func<Task> act = () => source.ForEachAsync((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenSourceWithElements_PerformsActionForAllElements_WithCorrectIndex() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEachAsync((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Count.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(Array.IndexOf(source, sourceItem));
                    }
                }

                private class SourceItem {
                    public static Dictionary<SourceItem, int> PerformedActions;

                    public SourceItem() {
                        ActionIndex = -1;
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public int ActionIndex { get; private set; }

                    public static void ResetPerformedActions() {
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public Task DoSomeAction(int index) {
                        ActionIndex = index;
                        PerformedActions[this] = index;
                        return Task.CompletedTask;
                    }
                }
            }

            public class AsyncForEach : EnumerableOfT {
                public AsyncForEach() {
                    SourceItem.ResetPerformedActionCount();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Func<Task> act = () => source.ForEach(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEach((Func<SourceItem, Task>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionPerformed.Should().BeFalse();
                    }
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Func<Task> act = () => source.ForEach(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenSourceWithElements_PerformsActionForAllElements() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEach(item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionPerformed.Should().BeTrue();
                    }
                }

                private class SourceItem {
                    public static int PerformedActionCount { get; private set; }

                    public bool ActionPerformed { get; private set; }

                    public static void ResetPerformedActionCount() {
                        PerformedActionCount = 0;
                    }

                    public Task DoSomeAction() {
                        ActionPerformed = true;
                        PerformedActionCount++;
                        return Task.CompletedTask;
                    }
                }
            }

            public class AsyncForEachWithIndex : EnumerableOfT {
                public AsyncForEachWithIndex() {
                    SourceItem.ResetPerformedActions();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Func<Task> act = () => source.ForEach((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEach((Func<SourceItem, int, Task>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(-1);
                    }
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Func<Task> act = () => source.ForEach((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenSourceWithElements_PerformsActionForAllElements_WithCorrectIndex() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Func<Task> act = () => source.ForEach((item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Count.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(Array.IndexOf(source, sourceItem));
                    }
                }

                private class SourceItem {
                    public static Dictionary<SourceItem, int> PerformedActions;

                    public SourceItem() {
                        ActionIndex = -1;
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public int ActionIndex { get; private set; }

                    public static void ResetPerformedActions() {
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public Task DoSomeAction(int index) {
                        ActionIndex = index;
                        PerformedActions[this] = index;
                        return Task.CompletedTask;
                    }
                }
            }

            public class DoActionIf : EnumerableOfT {
                public DoActionIf() {
                    SourceItem.ResetPerformedActionCount();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: (Action<SourceItem>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenPredicateIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: null,
                        action: item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(0);
                }

                [Fact]
                public void GivenSourceWithElements_AndPredicateIsTrue_PerformsActionForAllElements_WithCorrectIndex() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionPerformed.Should().BeTrue();
                    }
                }

                [Fact]
                public void GivenSourceWithElements_OnlyPerformsActionsForElementsForWhichThePredicateIsTrue_WithCorrectIndex() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: item => Array.IndexOf(source, item) < 1,
                        action: item => item.DoSomeAction());
                    act.Should().NotThrow();
                    SourceItem.PerformedActionCount.Should().Be(1);
                    source.ElementAt(0).ActionPerformed.Should().BeTrue();
                    source.ElementAt(1).ActionPerformed.Should().BeFalse();
                }

                private class SourceItem {
                    public static int PerformedActionCount { get; private set; }

                    public bool ActionPerformed { get; private set; }

                    public static void ResetPerformedActionCount() {
                        PerformedActionCount = 0;
                    }

                    public void DoSomeAction() {
                        ActionPerformed = true;
                        PerformedActionCount++;
                    }
                }
            }

            public class DoActionIfWithIndex : EnumerableOfT {
                public DoActionIfWithIndex() {
                    SourceItem.ResetPerformedActions();
                }

                [Fact]
                public void GivenSourceIsNull_DoesNotThrow_DoesNotPerformAction() {
                    IEnumerable<SourceItem> source = null;
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: (item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenActionIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: (Action<SourceItem, int>) null);
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(-1);
                    }
                }

                [Fact]
                public void GivenPredicateIsNull_DoesNotThrow_DoesNotPerformAction() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: null,
                        action: (item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(-1);
                    }
                }

                [Fact]
                public void GivenSourceIsEmpty_DoesNotThrow_DoesNotPerformAction() {
                    var source = Enumerable.Empty<SourceItem>();
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: (item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Should().BeEmpty();
                }

                [Fact]
                public void GivenSourceWithElements_AndPredicateIsTrue_PerformsActionForAllElements_WithCorrectIndex() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: item => true,
                        action: (item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Count.Should().Be(source.Length);
                    foreach (var sourceItem in source) {
                        sourceItem.ActionIndex.Should().Be(Array.IndexOf(source, sourceItem));
                    }
                }

                [Fact]
                public void GivenSourceWithElements_OnlyPerformsActionsForElementsForWhichThePredicateIsTrue_WithCorrectIndex() {
                    var source = new[] {new SourceItem(), new SourceItem()};
                    Action act = () => source.DoActionIf(
                        predicate: item => Array.IndexOf(source, item) < 1,
                        action: (item, idx) => item.DoSomeAction(idx));
                    act.Should().NotThrow();
                    SourceItem.PerformedActions.Count.Should().Be(1);
                    source.ElementAt(0).ActionIndex.Should().Be(0);
                    source.ElementAt(1).ActionIndex.Should().Be(-1);
                }

                private class SourceItem {
                    public static Dictionary<SourceItem, int> PerformedActions;

                    public SourceItem() {
                        ActionIndex = -1;
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public int ActionIndex { get; private set; }

                    public static void ResetPerformedActions() {
                        PerformedActions = new Dictionary<SourceItem, int>();
                    }

                    public void DoSomeAction(int index) {
                        ActionIndex = index;
                        PerformedActions[this] = index;
                    }
                }
            }
        }
    }
}