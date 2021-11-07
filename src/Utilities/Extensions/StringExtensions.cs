using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Extensions
{
    /// <summary>
    /// Class which contains extension methods for the String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to camel case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// camel cased string (first character upper case, remaining characters unchanged)
        /// </returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1)
            {
                return value;
            }

            return value.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture) + value.Substring(1);
        }

        /// <summary>
        /// Converts a string to pascal case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// pascal cased string (first character lower case, remaining characters unchanged)
        /// </returns>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 1)
            {
                return value;
            }

            return value.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + value.Substring(1);
        }

        /// <summary>
        /// Returns the first number of characters of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        /// <summary>
        /// Returns the last number of characters of a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || value.Length < length)
            {
                return value;
            }

            return value.Substring(value.Length - length);
        }

        /// <summary>
        /// Returns a default value when the string value is empty, or otherwise the current string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="actionWhenNull">Optional action to perform when the value is null.</param>
        /// <returns>
        /// Default value when null, otherwise the current value.
        /// </returns>
        public static string WhenNull(this string value, string defaultValue = "", Action actionWhenNull = null)
        {
            if (value == null && actionWhenNull != null)
            {
                actionWhenNull();
            }

            return value ?? defaultValue;
        }

        /// <summary>
        /// Fixes the name of the type.
        /// </summary>
        /// <param name="instance">The type name to fix.</param>
        /// <returns></returns>
        public static string FixTypeName(this string instance)
        {
            if (instance == null)
            {
                return null;
            }

            int startIndex;
            while (true)
            {
                startIndex = instance.IndexOf(", ");
                if (startIndex == -1)
                {
                    break;
                }

                int secondIndex = instance.IndexOf("]", startIndex + 1);
                if (secondIndex == -1)
                {
                    break;
                }

                instance = instance.Substring(0, startIndex) + instance.Substring(secondIndex + 1);
            }

            while (true)
            {
                startIndex = instance.IndexOf("`");
                if (startIndex == -1)
                {
                    break;
                }

                instance = instance.Substring(0, startIndex) + instance.Substring(startIndex + 2);
            }

            //remove assebmly name from type, e.g. System.String, mscorlib bla bla bla -> System.String
            var index = instance.IndexOf(", ");
            if (index > -1)
            {
                instance = instance.Substring(0, index);
            }

            return FixAnonymousTypeName(instance
                .Replace("[[", "<")
                .Replace(",[", ",")
                .Replace(",]", ">" /*","*/)
                .Replace("]", ">")
                .Replace("[>", "[]") //hacking here! caused by the previous statements...
                .Replace("System.Void", "void")
                .Replace("+", ".")
                .Replace("&", ""));
        }

        private static string FixAnonymousTypeName(string instance)
        {
            var isAnonymousType = instance.Contains("AnonymousType")
                && (instance.Contains("<>") || instance.Contains("VB$"));

            var arraySuffix = instance.EndsWith("[]")
                ? "[]"
                : string.Empty;

            return isAnonymousType
                ? $"AnonymousType{arraySuffix}"
                : instance;
        }

        private static readonly string[] _trueKeywords = new[] { "true", "t", "1", "y", "yes", "ja", "j"};
        private static readonly string[] _falseKeywords = new[] { "false", "f", "0", "n", "no", "nee" };

        /// <summary>
        /// Determines whether the specified instance is true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsTrue(this string instance)
        {
            return instance != null && _trueKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether the specified instance is false.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsFalse(this string instance)
        {
            return instance != null && _falseKeywords.Any(s => s.Equals(instance, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Performs a is null or empty check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrEmpty">The when null or empty.</param>
        /// <returns></returns>
        public static string WhenNullOrEmpty(this string instance, string whenNullOrEmpty)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return whenNullOrEmpty;
            }

            return instance;
        }

        /// <summary>
        /// Performs a is null or empty check, and returns another value when this evaluates to true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="whenNullOrEmptyDelegate">The delegate to invoke when null or empty.</param>
        /// <returns></returns>
        public static string WhenNullOrEmpty(this string instance, Func<string> whenNullOrEmptyDelegate)
        {
            if (whenNullOrEmptyDelegate == null)
            {
                throw new ArgumentNullException(nameof(whenNullOrEmptyDelegate));
            }

            if (string.IsNullOrEmpty(instance))
            {
                return whenNullOrEmptyDelegate();
            }

            return instance;
        }

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In(this string value, IEnumerable<string> values, StringComparison stringComparison)
        {
            return values.Any(i => i.Equals(value, stringComparison));
        }

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In(this string value, StringComparison stringComparison, params string[] values)
        {
            return values.Any(i => i.Equals(value, stringComparison));
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified Unicode character in this instance are replaced with another specified Unicode character.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns></returns>
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (newValue == null)
            {
                throw new ArgumentNullException(nameof(newValue));
            }

            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str, previousIndex, index - previousIndex);
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str, previousIndex, str.Length - previousIndex);

            return sb.ToString();
        }

        /// <summary>
        /// Sanitizes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static string Sanitize(this string token)
        {
            if (token == null)
            {
                return string.Empty;
            }

            // Replace all invalid chars by underscores 
            token = Regex.Replace(token, @"[\W\b]", "_", RegexOptions.IgnoreCase);

            // If it starts with a digit, prefix it with an underscore 
            token = Regex.Replace(token, @"^\d", "_$0");

            return token;
        }

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, params string[] values) =>
            instance.StartsWithAny((IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, IEnumerable<string> values) =>
            values.Any(v => instance.StartsWith(v));

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, StringComparison comparisonType, params string[] values) =>
            instance.StartsWithAny(comparisonType, (IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance starts with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool StartsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values) =>
            values.Any(v => instance.StartsWith(v, comparisonType));

        /// <summary>
        /// Indicates whether the string instance ends with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, params string[] values) =>
            instance.EndsWithAny((IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance ends with any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, IEnumerable<string> values) =>
            values.Any(v => instance.EndsWith(v));

        /// <summary>
        /// Indicates whether the string instance ends any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, StringComparison comparisonType, params string[] values) =>
            instance.EndsWithAny(comparisonType, (IEnumerable<string>)values);

        /// <summary>
        /// Indicates whether the string instance ends any of the specified values.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool EndsWithAny(this string instance, StringComparison comparisonType, IEnumerable<string> values) =>
            values.Any(v => instance.EndsWith(v, comparisonType));

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <param name="fullyQualifiedClassName">Fully qualified class name.</param>
        /// <returns></returns>
        public static string GetClassNameWithDefault(this string fullyQualifiedClassName)
        {
            var idx = fullyQualifiedClassName.LastIndexOf(".");
            return idx == -1
                ? fullyQualifiedClassName
                : fullyQualifiedClassName.Substring(idx + 1);
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <param name="fullyQualifiedClassName">Fully qualified class name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetNamespaceWithDefault(this string fullyQualifiedClassName, string defaultValue)
        {
            var idx = fullyQualifiedClassName.LastIndexOf(".");
            return idx == -1
                ? defaultValue
                : fullyQualifiedClassName.Substring(0, idx).WhenNullOrEmpty(defaultValue);
        }

        public static char? GetCharacterAt(this string value, int position)
            => position >= value.Length
                ? (char?)null
                : value[position];

        public static string RemoveSuffix(this string instance, string suffix)
        {
            if (suffix == null)
            {
                throw new ArgumentNullException(nameof(suffix));
            }

            return instance.EndsWith(suffix)
                ? instance.Substring(0, instance.Length - suffix.Length)
                : instance;
        }

        public static int OccurencesOf(this string instance, string textToFind)
        {
            int result = 0;
            int previous = -1;

            while (true)
            {
                int index = instance.IndexOf(textToFind, previous + 1);
                if (index == -1)
                {
                    break;
                }
                else
                {
                    result++;
                    previous = index;
                }
            }

            return result;
        }

        public static int OccurencesOf(this string instance, string textToFind, StringComparison stringComparison)
        {
            int result = 0;
            int previous = -1;

            while (true)
            {
                int index = instance.IndexOf(textToFind, previous + 1, stringComparison);
                if (index == -1)
                {
                    break;
                }
                else
                {
                    result++;
                    previous = index;
                }
            }

            return result;
        }

        public static string MakeGenericTypeName(this string instance, string genericTypeParameter)
            => string.IsNullOrEmpty(genericTypeParameter)
                ? instance
                : $"{instance}<{genericTypeParameter}>";

        public static bool IsUnitTestAssembly(this string instance)
            => instance.StartsWithAny(StringComparison.OrdinalIgnoreCase,
                                      "Microsoft.VisualStudio",
                                      "Microsoft.TestPlatform",
                                      "xunit",
                                      "testhost",
                                      "msdia140");

        /// <summary>
        /// Removes generics from a typename. (`1)
        /// </summary>
        /// <param name="typeName">Typename with or without generics</param>
        /// <returns>Typename without generics (`1)</returns>
        public static string WithoutGenerics(this string typeName)
        {
            if (typeName == null)
            {
                return typeName;
            }

            var index = typeName.IndexOf('`');
            return index == -1
                ? typeName
                : typeName.Substring(0, index);
        }
    }
}
