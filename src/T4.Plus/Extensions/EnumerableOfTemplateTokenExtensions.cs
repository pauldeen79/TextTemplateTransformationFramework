using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Extensions
{
    public static class EnumerableOfTemplateTokenExtensions
    {
        public static string GetModelTypeName<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<IModelTypeToken<TState>>()
                .Select(t => t.ModelTypeName)
                .Distinct()
                .LastOrDefault(); //design decision: when multiple values are found, use the last one

        public static bool GetUseModelForRoutingOnly<TState>(this IEnumerable<ITemplateToken<TState>> instance, string modelTypeName)
            where TState : class
            => instance
                .OfType<IModelTypeToken<TState>>()
                .Any(t => t.ModelTypeName == modelTypeName && t.UseForRoutingOnly); //design decision: when any model type token has this set to true, then take this value.

        public static bool GetUseModelForRouting<TState>(this IEnumerable<ITemplateToken<TState>> instance, string modelTypeName)
            where TState : class
            => instance
                .OfType<IModelTypeToken<TState>>()
                .Any(t => t.ModelTypeName == modelTypeName && t.UseForRouting); //design decision: when any model type token has this set to true, then take this value.

        public static string GetTemplateName<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<ITemplateNameToken<TState>>()
                .Select(t => t.TemplateName)
                .Distinct()
                .LastOrDefault(); //design decision: when multiple values are found, use the last one

        public static string GetRootClassName<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance.GetTemplateBaseClassName
            (
                instance
                    .OfType<IBaseClassInheritsFromToken<TState>>()
                    .Select(t => t.ClassName)
                    .Distinct()
                    .LastOrDefault() ?? "GeneratedTemplateBase"
            );

        public static string GetClassName<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance.GetTemplateClassName
            (
                instance
                    .OfType<ITemplateClassNameToken<TState>>()
                    .Select(t => t.ClassName)
                    .Distinct()
                    .LastOrDefault() ?? "GeneratedTemplate"
            );

        public static bool GetSkipInitializationCode<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<ISkipInitializationCodeToken<TState>>()
                .Any();

        public static string GetBaseClassInheritsFrom<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<IBaseClassInheritsFromToken<TState>>()
                .Select(t => t.ClassName)
                .FirstOrDefault();

        public static bool GetEnableAdditionalActionDelegate<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<ICallAdditionalActionDelegateToken<TState>>()
                .Any();

        public static bool GetAddTemplateLineNumbers<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<IAddTemplateLineNumbersToken<TState>>()
                .Any(t => t.Enabled);

        public static bool GetAddExcludeFromCodeCoverage<TState>(this IEnumerable<ITemplateToken<TState>> instance)
            where TState : class
            => instance
                .OfType<IAddExcludeFromCodeCoverageAttributesToken<TState>>()
                .Distinct(t => t.Enabled)
                .LastOrDefault(); //design decision: when multiple values are found, use the last one
    }
}
