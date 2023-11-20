using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the specified section is a directive.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="name">The name.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>
        ///   <c>true</c> if the specified section is a directive; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDirective(this string section, string name, string prefix, string suffix)
            => section
                .TrimStart()
                .StartsWith(string.Format("{0}{1}{2}", prefix, name, suffix), StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the language token from the string value.
        /// </summary>
        /// <param name="instance">String value containing the language.</param>
        /// <param name="context">The secction context.</param>
        /// <returns>
        /// Null when the language == null or empty, initialize error token when the language is unknown, or else a language token.
        /// </returns>
        public static ITemplateToken<TState> GetLanguageToken<TState>(this string instance, SectionContext<TState> context)
            where TState : class
            => instance switch
            {
                var x when string.IsNullOrEmpty(x) => null,
                var x when x.Equals("c#", StringComparison.OrdinalIgnoreCase) => new LanguageToken<TState>(context, Language.CSharp, instance),
                var x when x.Equals("vb", StringComparison.OrdinalIgnoreCase) => new LanguageToken<TState>(context, Language.VbNet, instance),
                _ => new InitializeErrorToken<TState>(context, "Unsupported language: " + instance)
            };

        public static string GetAssemblyName(this string instance)
            => instance.IndexOf(", ") > -1
#if NETFRAMEWORK
                ? instance.Substring(0, instance.IndexOf(", ")) + ".dll"
#else
                ? string.Concat(instance.AsSpan(0, instance.IndexOf(", ")), ".dll")
#endif
                : instance;

        public static bool IsFullyQualifiedAssemblyName(this string instance)
            => instance.IndexOf(", ") > -1;

        public static IEnumerable<string> GetDirectories(this string instance, bool recursive)
            => !recursive
                ? new[] { instance }
                : new[] { instance }.Concat(Directory.GetDirectories(instance, "*", SearchOption.AllDirectories));
    }
}
