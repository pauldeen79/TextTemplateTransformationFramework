using System;
using System.Reflection;
using System.Runtime.Versioning;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the literal.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="isLiteral">if set to <c>true</c> [is literal].</param>
        /// <returns>
        /// String formatted as literal when it's a literal, or as an expression (unmodified) otherwise.
        /// </returns>
        public static string FormatLiteral(this string instance, bool isLiteral)
            => isLiteral
                ? instance.CsharpFormat()
                : instance;

        public static bool IsValidFrameworkVersion(this string frameworkFilter)
            => string.IsNullOrEmpty(frameworkFilter)
            || Assembly
                .GetEntryAssembly()?
                .GetCustomAttribute<TargetFrameworkAttribute>()?
                .GetFrameworkName()?
                .StartsWith(frameworkFilter, StringComparison.OrdinalIgnoreCase) == true;
    }
}
