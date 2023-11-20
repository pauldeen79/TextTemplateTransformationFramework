using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Utilities.Extensions
{
    /// <summary>
    /// Class which contains extension methods for the Object class.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts an object value to string with null check.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// value.ToString() when the value is not null, string.Empty otherwise.
        /// </returns>
        public static string ToStringWithNullCheck(this object value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var toStringMethods = value.GetType().GetMethods().Where(x => x.Name == "ToString" && x.GetParameters().Length > 0 && Array.TrueForAll(x.GetParameters(), y => y.Name == "provider")).ToArray();
            if (toStringMethods.Length == 1)
            {
                return (string)toStringMethods[0].Invoke(value, new[] { CultureInfo.InvariantCulture });
            }

            return value.ToString();
        }

        /// <summary>
        /// Converts an object value to string with default value if null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// value.ToString() when te value is not null, defaultValue otherwise.
        /// </returns>
        public static string ToStringWithDefault(this object value, string defaultValue = null)
        {
            if (value is null)
            {
                return defaultValue;
            }

            var toStringMethods = value.GetType().GetMethods().Where(x => x.Name == "ToString" && x.GetParameters().Length > 0 && Array.TrueForAll(x.GetParameters(), y => y.Name == "provider")).ToArray();
            if (toStringMethods.Length == 1)
            {
                return (string)toStringMethods[0].Invoke(value, new[] { CultureInfo.InvariantCulture });
            }

            return value.ToString();
        }

        /// <summary>
        /// Formats the value as a CSharp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Csharp formatted value.
        /// </returns>
        public static string CsharpFormat(this object value)
        {
            if (value is null)
            {
                return "null";
            }

            if (value is string x)
            {
                return "@\"" + (x).Replace("\"", "\"\"") + "\"";
            }

            if (value is bool x2)
            {
                return (x2)
                    ? "true"
                    : "false";
            }

            var toStringMethods = value.GetType().GetMethods().Where(x => x.Name == "ToString" && x.GetParameters().Length > 0 && Array.TrueForAll(x.GetParameters(), y => y.Name == "provider")).ToArray();
            if (toStringMethods.Length == 1)
            {
                return (string)toStringMethods[0].Invoke(value, new[] { CultureInfo.InvariantCulture });
            }

            return value.ToString();
        }

        /// <summary>
        /// Try-validates the object, and returns the valid state.
        /// </summary>
        /// <param name="instance">Object to validate.</param>
        /// <param name="validationResults">Validation results.</param>
        /// <returns>
        /// true when valid, otherwise false.
        /// </returns>
        public static bool TryValidate(this object instance, out ICollection<ValidationResult> validationResults)
        {
            validationResults = new Collection<ValidationResult>();
            return Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), validationResults, true);
        }

        public static T WithDefaultValues<T>(this T instance)
        {
            var properties = instance
                .GetType()
                .GetProperties()
                .Where
                (
                    p => p.CanRead
                        && p.CanWrite && p.GetGetMethod() is not null
                        && p.GetSetMethod() is not null
                        && p.GetCustomAttribute<DefaultValueAttribute>(true) is not null
                );

            properties.ForEach
            (
                property => property.SetValue(instance, property.GetCustomAttribute<DefaultValueAttribute>(true).Value, null)
            );

            return instance;
        }

        public static TResult Either<T, TResult>(this T instance, Func<T, TResult> valueDelegate, Func<TResult> defaultValueDelegate)
        {
            if (valueDelegate is null)
            {
                throw new ArgumentNullException(nameof(valueDelegate));
            }

            if (defaultValueDelegate is null)
            {
                throw new ArgumentNullException(nameof(defaultValueDelegate));
            }

            return EqualityComparer<T>.Default.Equals(instance, default)
                ? defaultValueDelegate()
                : valueDelegate(instance);
        }

        public static TResult Either<T, TResult>(this T instance, Func<T, bool> predicate, Func<T, TResult> applyDelegate, Func<T, TResult> defaultValueDelegate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (applyDelegate is null)
            {
                throw new ArgumentNullException(nameof(applyDelegate));
            }

            if (defaultValueDelegate is null)
            {
                throw new ArgumentNullException(nameof(defaultValueDelegate));
            }

            return predicate(instance)
                ? applyDelegate(instance)
                : defaultValueDelegate(instance);
        }

        public static TResult Apply<T, TResult>(this T instance, Func<T, TResult> applyDelegate)
        {
            if (applyDelegate is null)
            {
                throw new ArgumentNullException(nameof(applyDelegate));
            }

            return applyDelegate(instance);
        }

        public static T Then<T>(this T instance, Action<T> actionDelegate)
        {
            if (actionDelegate is null)
            {
                throw new ArgumentNullException(nameof(actionDelegate));
            }
            
            actionDelegate(instance);
            return instance;
        }

        public static T With<T>(this T instance, Func<T, T> actionDelegate)
        {
            if (actionDelegate is null)
            {
                throw new ArgumentNullException(nameof(actionDelegate));
            }

            return actionDelegate(instance);
        }

        public static IEnumerable<T> AsEnumerable<T>(this T instance) =>
            new[] { instance };
    }
}
