using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Dalion.Ringor.Utils {
    public static partial class Extensions {
        /// <summary>
        ///     Create a shallow clone of the specified subject, and change the value of the specified property.
        /// </summary>
        /// <param name="subject">The subject to clone.</param>
        /// <param name="expression">An expression that points to a member of the subject to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>
        ///     A shallow clone of the specified subject, with the specified value set to the properto to which the specified
        ///     expression points.
        /// </returns>
        public static TSubject With<TSubject, TValue>(this TSubject subject, Expression<Func<TSubject, TValue>> expression, TValue value) {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var member = expression.GetTargetMemberInfo();
            if (member == null) throw new ArgumentException("The specified expression does not point to a valid member.", nameof(expression));

            member.SetMemberValue(subject, value);

            return subject;
        }

        /// <summary>
        ///     Create a shallow clone of the specified subject.
        /// </summary>
        /// <param name="subject">The subject to clone.</param>
        /// <returns>
        ///     A shallow clone of the specified subject.
        /// </returns>
        public static TSubject Clone<TSubject>(this TSubject subject) {
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            object clone;
            var cloneable = subject as ICloneable;
            if (cloneable != null) {
                clone = cloneable.Clone();
            }
            else {
                var memberwiseCloneMethod = subject.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new Type[0], null);
                clone = memberwiseCloneMethod.Invoke(subject, Array.Empty<object>());
            }

            return (TSubject) clone;
        }

        /// <summary>
        ///     Create a shallow clone of the specified subject, and change the value of the specified property.
        /// </summary>
        /// <param name="subject">The subject to clone.</param>
        /// <param name="expression">An expression that points to a member of the subject to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>
        ///     A shallow clone of the specified subject, with the specified value set to the properto to which the specified
        ///     expression points.
        /// </returns>
        public static TSubject CloneWith<TSubject, TValue>(this TSubject subject, Expression<Func<TSubject, TValue>> expression, TValue value) {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var member = expression.GetTargetMemberInfo();
            if (member == null) throw new ArgumentException("The specified expression does not point to a valid member.", nameof(expression));

            object clone;
            var cloneable = subject as ICloneable;
            if (cloneable != null) {
                clone = cloneable.Clone();
            }
            else {
                var memberwiseCloneMethod = subject.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new Type[0], null);
                clone = memberwiseCloneMethod.Invoke(subject, Array.Empty<object>());
            }
            member.SetMemberValue(clone, value);

            return (TSubject) clone;
        }

        /// <summary>
        ///     Sets the member's value on the target object.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="subject">The subject that contains the target.</param>
        /// <param name="value">The value to set.</param>
        public static void SetMemberValue(this MemberInfo member, object subject, object value) {
            if (member == null) throw new ArgumentNullException(nameof(member));

            switch (member.MemberType) {
                case MemberTypes.Field:
                    ((FieldInfo) member).SetValue(subject, value);
                    break;
                case MemberTypes.Property:
                    var property = (PropertyInfo) member;
                    if (!property.CanWrite) throw new ArgumentException($"Cannot write to property '{property.Name}'.", nameof(member));
                    if (value != null && !property.PropertyType.IsInstanceOfType(value)) {
                        try {
                            value = Convert.ChangeType(value, property.PropertyType);
                        }
                        catch (InvalidCastException) {
                            throw new ArgumentException($"Cannot assign a value of type '{value.GetType().Name}' to property '{property.Name}' (type: {property.PropertyType.Name}).", nameof(value));
                        }
                    }
                    property.SetValue(subject, value, null);
                    break;
                default:
                    throw new ArgumentException($"The specified member must be of type {typeof(FieldInfo).Name} or {typeof(PropertyInfo).Name}.", nameof(member));
            }
        }

        /// <summary>
        ///     Retrieves the member that an expression is defined for.
        /// </summary>
        /// <param name="expression">The expression to retrieve the member from.</param>
        /// <returns>A <see cref="MemberInfo" /> instance if the member could be found; otherwise <see langword="null" />.</returns>
        public static MemberInfo GetTargetMemberInfo(this Expression expression) {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            switch (expression.NodeType) {
                case ExpressionType.Convert:
                    return GetTargetMemberInfo(((UnaryExpression) expression).Operand);
                case ExpressionType.Lambda:
                    return GetTargetMemberInfo(((LambdaExpression) expression).Body);
                case ExpressionType.Call:
                    return ((MethodCallExpression) expression).Method;
                case ExpressionType.MemberAccess:
                    return ((MemberExpression) expression).Member;
                default:
                    return null;
            }
        }
    }
}