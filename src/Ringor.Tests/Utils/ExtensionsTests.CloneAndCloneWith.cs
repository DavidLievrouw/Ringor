using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Dalion.Ringor.Utils {
    public partial class ExtensionsTests {
        public class With : ExtensionsTests {
            [Fact]
            public void GivenSubjectIsNull_Throws() {
                Action act = () => Extensions.With<Bar, string>(null, _ => _.Value, "x");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenExpressionIsNull_Throws() {
                Action act = () => new Bar().With(null, "x");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenExpressionDoesNotPointToAFieldOrAProperty_Throws() {
                Action act = () => new Bar().With(_ => _.GetValue(), "x");
                act.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void GivenNullValue_SetsMemberToNull() {
                var bar = new Bar {Value = "XYZ"};
                var actual = bar.With(_ => _.Value, null);
                actual.Value.Should().BeNull();
                actual.Should().BeSameAs(bar);
            }

            [Fact]
            public void GivenNonNullValue_SetsMemberToSpecifiedValue() {
                var bar = new Bar();
                var actual = bar.With(_ => _.Value, "ABC");
                actual.Value.Should().Be("ABC");
                actual.Should().BeSameAs(bar);
            }
        }

        public class CloneICloneable : ExtensionsTests {
            [Fact]
            public void GivenSubjectIsNull_Throws() {
                Action act = () => Extensions.Clone<CloneableBar>(null);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Clones() {
                var bar = new CloneableBar {Value = "XYZ"};
                var actual = bar.Clone();
                actual.Should().BeEquivalentTo(bar);
                actual.Should().NotBeSameAs(bar);
            }
        }

        public class CloneNonICloneable : ExtensionsTests {
            [Fact]
            public void GivenSubjectIsNull_Throws() {
                Action act = () => Extensions.Clone<Bar>(null);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void Clones() {
                var bar = new Bar {Value = "XYZ"};
                var actual = bar.Clone();
                actual.Should().BeEquivalentTo(bar);
                actual.Should().NotBeSameAs(bar);
            }
        }

        public class CloneWithICloneable : ExtensionsTests {
            [Fact]
            public void GivenSubjectIsNull_Throws() {
                Action act = () => Extensions.CloneWith<CloneableBar, string>(null, _ => _.Value, "x");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenExpressionIsNull_Throws() {
                Action act = () => new CloneableBar().CloneWith(null, "x");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenExpressionDoesNotPointToAFieldOrAProperty_Throws() {
                Action act = () => new CloneableBar().CloneWith(_ => _.GetValue(), "x");
                act.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void GivenNullValue_Clones_AndSetsMemberToNull() {
                var bar = new CloneableBar {Value = "XYZ"};
                var actual = bar.CloneWith(_ => _.Value, null);
                actual.Value.Should().BeNull();
                actual.Should().NotBeSameAs(bar);
            }

            [Fact]
            public void GivenNonNullValue_Clones_AndSetsMemberToSpecifiedValue() {
                var bar = new CloneableBar();
                var actual = bar.CloneWith(_ => _.Value, "ABC");
                actual.Value.Should().Be("ABC");
                actual.Should().NotBeSameAs(bar);
            }
        }

        public class CloneWithNonICloneable : ExtensionsTests {
            [Fact]
            public void GivenSubjectIsNull_Throws() {
                Action act = () => Extensions.CloneWith<Bar, string>(null, _ => _.Value, "x");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenExpressionIsNull_Throws() {
                Action act = () => new Bar().CloneWith(null, "x");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenExpressionDoesNotPointToAFieldOrAProperty_Throws() {
                Action act = () => new Bar().CloneWith(_ => _.GetValue(), "x");
                act.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void GivenNullValue_Clones_AndSetsMemberToNull() {
                var bar = new Bar {Value = "XYZ"};
                var actual = bar.CloneWith(_ => _.Value, null);
                actual.Value.Should().BeNull();
                actual.Should().NotBeSameAs(bar);
            }

            [Fact]
            public void GivenNonNullValue_Clones_AndSetsMemberToSpecifiedValue() {
                var bar = new Bar();
                var actual = bar.CloneWith(_ => _.Value, "ABC");
                actual.Value.Should().Be("ABC");
                actual.Should().NotBeSameAs(bar);
            }
        }

        public class SetMemberValue : ExtensionsTests {
            [Fact]
            public void WhenMemberIsNull_Throws() {
                Action act = () => Extensions.SetMemberValue(null, new Bar(), "x");
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void WhenPropertyIsNotOfSubject_Throws() {
                var property = typeof(SqlException).GetProperty("Number", BindingFlags.Instance | BindingFlags.Public);
                Action act = () => property.SetMemberValue(new Bar(), "x");
                act.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void WhenMemberIsField_SetsValue() {
                var field = typeof(Bar).GetField("Field", BindingFlags.Instance | BindingFlags.Public);
                var bar = new Bar();
                field.SetMemberValue(bar, 100);
                bar.Field.Should().Be(100);
            }

            [Fact]
            public void WhenMemberIsProperty_SetsValue() {
                var propertyInfo = typeof(Bar).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                var bar = new Bar();
                propertyInfo.SetMemberValue(bar, 100);
                bar.Id.Should().Be(100);
            }

            [Fact]
            public void WhenValueIsNotOfCorrectType_ButItCanBeConverted_ConvertsAndAssigns() {
                var propertyInfo = typeof(Bar).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                short shortValue = 3;
                var bar = new Bar();
                propertyInfo.SetMemberValue(bar, shortValue);
                bar.Id.Should().Be(shortValue);
            }

            [Fact]
            public void WhenValueIsNotOfCorrectType_AndItCannotBeConverted_Throws() {
                var someLinqQuery = new[] {1, 2}.Take(1);
                var collectionPropertyInfo = typeof(Bar).GetProperty("Collection", BindingFlags.Instance | BindingFlags.Public);
                var bar = new Bar();
                Action act = () => collectionPropertyInfo.SetMemberValue(bar, someLinqQuery);
                act.Should().Throw<ArgumentException>();
            }

            [Fact]
            public void WhenSubjectIsNull_SetsStaticMember() {
                var propertyInfo = typeof(Bar).GetProperty("Counter", BindingFlags.Static | BindingFlags.Public);
                propertyInfo.SetMemberValue(null, 42);
                Bar.Counter.Should().Be(42);
            }

            [Fact]
            public void WhenSubjectIsNull_AndTargetIsNotStatic_Throws() {
                var propertyInfo = typeof(Bar).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                Action act = () => propertyInfo.SetMemberValue(null, 42);
                act.Should().Throw<TargetException>();
            }

            [Fact]
            public void WhenMemberIsNotFieldOrProperty_Throws() {
                var bar = new Bar();
                var methodInfo = typeof(Bar).GetMethod("GetValue");
                Action act = () => methodInfo.SetMemberValue(bar, "ABC");
                act.Should().Throw<ArgumentException>();
            }
        }

        public class GetTargetMemberInfo : ExtensionsTests {
            [Fact]
            public void GivenExpressionIsNull_Throws() {
                Action act = () => Extensions.GetTargetMemberInfo(null);
                act.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void GivenExpressionIsConvert_RecursesOnOperand() {
                var paramExpr = Expression.Parameter(typeof(Bar), "bar");
                var propertyExpr = Expression.Property(paramExpr, "Id");
                var convertExpr = Expression.Convert(propertyExpr, typeof(short));

                var actual = convertExpr.GetTargetMemberInfo();

                var expected = typeof(Bar).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                actual.Should().NotBeNull().And.BeAssignableTo<PropertyInfo>();
                actual.Name.Should().Be(expected.Name);
                actual.ReflectedType.Should().Be(expected.ReflectedType);
            }

            [Fact]
            public void GivenExpressionIsLambda_RecursesOnBody() {
                var paramExpr = Expression.Parameter(typeof(Bar), "bar");
                var propertyExpr = Expression.Property(paramExpr, "Id");
                var lambdaExpr = Expression.Lambda<Func<Bar, int>>(propertyExpr, paramExpr);

                var actual = lambdaExpr.GetTargetMemberInfo();

                var expected = typeof(Bar).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                actual.Should().NotBeNull().And.BeAssignableTo<PropertyInfo>();
                actual.Name.Should().Be(expected.Name);
                actual.ReflectedType.Should().Be(expected.ReflectedType);
            }

            [Fact]
            public void GivenExpressionIsCall_ReturnsMethod() {
                var paramExpr = Expression.Parameter(typeof(Bar), "bar");
                var fortyTwo = Expression.Constant(42);
                var callExpr = Expression.Call(paramExpr, typeof(Bar).GetMethod("SetId"), fortyTwo);

                var actual = callExpr.GetTargetMemberInfo();

                var expected = typeof(Bar).GetMethod("SetId");
                actual.Should().NotBeNull().And.BeAssignableTo<MethodInfo>();
                actual.Name.Should().Be(expected.Name);
                actual.ReflectedType.Should().Be(expected.ReflectedType);
            }

            [Fact]
            public void GivenExpressionIsField_ReturnsMember() {
                var paramExpr = Expression.Parameter(typeof(Bar), "bar");
                var fieldExp = Expression.Field(paramExpr, typeof(Bar), "Field");

                var actual = fieldExp.GetTargetMemberInfo();

                var expected = typeof(Bar).GetField("Field", BindingFlags.Instance | BindingFlags.Public);
                actual.Should().NotBeNull().And.BeAssignableTo<FieldInfo>();
                actual.Name.Should().Be(expected.Name);
                actual.ReflectedType.Should().Be(expected.ReflectedType);
            }

            [Fact]
            public void GivenExpressionIsProperty_ReturnsMember() {
                var paramExpr = Expression.Parameter(typeof(Bar), "bar");
                var propertyExpr = Expression.Property(paramExpr, "Id");

                var actual = propertyExpr.GetTargetMemberInfo();

                var expected = typeof(Bar).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);
                actual.Should().NotBeNull().And.BeAssignableTo<PropertyInfo>();
                actual.Name.Should().Be(expected.Name);
                actual.ReflectedType.Should().Be(expected.ReflectedType);
            }

            [Fact]
            public void GivenExpressionCannotBeDeducedToAFieldMethodOrProperty_ReturnsNull() {
                var constantExpr = Expression.Constant(42);
                var actual = constantExpr.GetTargetMemberInfo();
                actual.Should().BeNull();
            }

            [Fact]
            public void GivenExpressionCannotBeDeducedToAFieldMethodOrProperty_EvenWhenRecursing_ReturnsNull() {
                var paramExpr = Expression.Parameter(typeof(Bar), "bar");
                var propExpr = Expression.Property(paramExpr, "Id");
                var constExpr = Expression.Constant(42);
                var equalsExpr = Expression.Equal(propExpr, constExpr);
                var lambda = Expression.Lambda<Func<Bar, bool>>(equalsExpr, paramExpr);

                var actual = lambda.GetTargetMemberInfo();
                actual.Should().BeNull();
            }
        }

        public class Bar {
            public int Field;
            public int Id { get; set; }
            public string Value { get; set; }
            public static int Counter { get; set; }
            public int[] Collection { get; set; }

            public string GetValue() {
                return Value;
            }

            public void SetId(int id) {
                Id = id;
            }
        }

        public class CloneableBar : Bar, ICloneable {
            public object Clone() {
                return MemberwiseClone();
            }
        }
    }
}