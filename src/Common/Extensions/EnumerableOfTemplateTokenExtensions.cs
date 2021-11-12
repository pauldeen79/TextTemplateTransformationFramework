using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class EnumerableOfTemplateTokenExtensions
    {
        /// <summary>
        /// Gets the template tokens from the specified source section tokens enumerable.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IEnumerable<ITemplateToken<TState>> GetTemplateTokensFromSections<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance.GetTemplateTokensFromSections<TState, ITemplateToken<TState>>();

        /// <summary>
        /// Gets the template tokens from the specified source section tokens enumerable.
        /// </summary>
        /// <typeparam name="T">Type filter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetTemplateTokensFromSections<TState, T>(this IEnumerable<ITemplateToken<TState>> instance)
            where T : ITemplateToken<TState>
            where TState : class
            => instance
                .OfType<ISourceSectionToken<TState>>()
                .SelectMany(t => t.TemplateTokens)
                .OfType<T>();

        /// <summary>
        /// Determines whether the specified source section tokens enumerable has any template token that matches the specified type and predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///   <c>true</c> if [has template token in sections] [the specified predicate]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasTemplateTokenInSections<TState, T>(this IEnumerable<ITemplateToken<TState>> instance, Func<T, bool> predicate)
            where T : ITemplateToken<TState>
            where TState : class
            => GetTemplateTokensFromSections<TState, T>(instance).Any(predicate);

        /// <summary>
        /// Gets the template class name from the specified source section tokens.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetTemplateClassName<TState>(this IEnumerable<ITemplateToken<TState>> instance, string defaultValue = "GeneratedClass")
            where TState : class
            => instance
                .OfType<ITemplateClassNameToken<TState>>()
                .Select(t => t.ClassName)
                .Distinct()
                .LastOrDefaultWhenEmpty(defaultValue); //design decision: when multiple values are found, use the last one

        /// <summary>
        /// Gets the template baseclass name from the specified source section tokens.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetTemplateBaseClassName<TState>(this IEnumerable<ITemplateToken<TState>> instance, string defaultValue)
            where TState : class
            => instance
                .OfType<ITemplateClassNameToken<TState>>()
                .Select(t => t.BaseClassName.WhenNullOrEmpty(t.ClassName + "Base"))
                .Distinct()
                .LastOrDefaultWhenEmpty(defaultValue); //design decision: when multiple values are found, use the last one

        public static string GetCultureCode<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<ICultureToken<TState>>()
                .Distinct(t => t.Code)
                .LastOrDefault(); //design decision: when multiple values are found, use the last one

        public static bool GetTemplateIsOverride<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance.OfType<IOverrideTemplateToken<TState>>().Any();

        public static string GetGenerationEnvironmentAccessor<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance.OfType<ITemplateGenerationEnvironmentAccessorToken<TState>>()
                .Distinct(t => t.GenerationEnvironmentAccessor)
                .LastOrDefaultWhenEmpty("protected"); //design decision: when multiple values are found, use the last one

        public static string GetEnvironmentVersion<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<IEnvironmentVersionToken<TState>>()
                .Distinct(t => t.Value)
                .LastOrDefault(); //design decision: when multiple values are found, use the last one

        public static string GetTemplateNamespace<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<ITemplateNamespaceToken<TState>>()
                .Distinct(t => t.Namespace)
                .LastOrDefaultWhenEmpty("GeneratedNamespace"); //design decision: when multiple values are found, use the last one

        public static Language GetLanguage<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<ILanguageToken<TState>>()
                .Distinct(t => t.Value)
                .LastOrDefault(); //design decision: when multiple values are found, use the last one

        public static string GetOutputExtension<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<IOutputExtensionToken<TState>>()
                .Distinct(t => t.Extension)
                .LastOrDefault(); //design decision: when multiple values are found, use the last one

        public static string GetTempPath<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<ITempPathToken<TState>>()
                .Distinct(t => t.Value)
                .LastOrDefault(); //design decision: when multiple values are found, use the last one
    }
}
