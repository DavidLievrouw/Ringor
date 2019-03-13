using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class ForEach : ExtensionsTests {
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

            [Fact]
            public void ReturnsUnchangedSource() {
                var source = new[] {new SourceItem(), new SourceItem()};
                var actual = source.ForEach(item => item.DoSomeAction());
                actual.Should().Equal(source);
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

        public class ForEachWithIndex : ExtensionsTests {
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

            [Fact]
            public void ReturnsUnchangedSource() {
                var source = new[] {new SourceItem(), new SourceItem()};
                var actual = source.ForEach((item, idx) => item.DoSomeAction(idx));
                actual.Should().Equal(source);
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

        public class ForEachAsyncWithoutAsyncSuffix : ExtensionsTests {
            public ForEachAsyncWithoutAsyncSuffix() {
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

            [Fact]
            public async Task ReturnsUnchangedSource() {
                var source = new[] {new SourceItem(), new SourceItem()};
                var actual = await source.ForEach(item => item.DoSomeAction());
                actual.Should().Equal(source);
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

        public class ForEachAsync : ExtensionsTests {
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

            [Fact]
            public async Task ReturnsUnchangedSource() {
                var source = new[] {new SourceItem(), new SourceItem()};
                var actual = await source.ForEachAsync(item => item.DoSomeAction());
                actual.Should().Equal(source);
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

        public class ForEachAsyncWithoutAsyncSuffixWithIndex : ExtensionsTests {
            public ForEachAsyncWithoutAsyncSuffixWithIndex() {
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

            [Fact]
            public async Task ReturnsUnchangedSource() {
                var source = new[] {new SourceItem(), new SourceItem()};
                var actual = await source.ForEach((item, idx) => item.DoSomeAction(idx));
                actual.Should().Equal(source);
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

        public class ForEachAsyncWithIndex : ExtensionsTests {
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

            [Fact]
            public async Task ReturnsUnchangedSource() {
                var source = new[] {new SourceItem(), new SourceItem()};
                var actual = await source.ForEachAsync((item, idx) => item.DoSomeAction(idx));
                actual.Should().Equal(source);
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
    }
}