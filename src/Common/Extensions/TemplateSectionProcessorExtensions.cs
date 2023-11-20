using System;
using System.Collections.Generic;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.Common.Models;
using TextTemplateTransformationFramework.T4.Common.Contracts;
using Utilities;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TemplateSectionProcessorExtensions
    {
        /// <summary>
        /// Determines whether the current template section processor is a processor for the specified section.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="context">The section context.</param>
        /// <returns>
        ///   <c>true</c> if [is processor for section] [the specified section]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsProcessorForSection<TState>(this ITemplateSectionProcessor<TState> instance, SectionContext<TState> context)
            where TState : class
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return instance.GetType().GetModelType(typeof(TState)) switch
            {
                var type when type.GetCustomAttribute<AllowAllSectionsAttribute>(true) is not null => true,
#if NETFRAMEWORK
                    var type when context.Section.StartsWith("@") =>
#else
                    var type when context.Section.StartsWith('@') =>
#endif
                ScopedMember.Evaluate
                (
                    type.GetCustomAttribute<DirectivePrefixAttribute>(true),
                    directivePrefixAttribute =>
                        directivePrefixAttribute is null
                            ? typeof(ITemplateCustomDirectiveName).IsAssignableFrom(type)
                                && context.TokenParserCallback.SectionIsDirectiveWithName(context, ((ITemplateCustomDirectiveName)instance).TemplateCustomDirectiveName)
                            : context.TokenParserCallback.SectionIsDirectiveWithName(context, directivePrefixAttribute.Name)
                ),
                var type => ScopedMember.Evaluate
                (
                    type.GetCustomAttribute<SectionPrefixAttribute>(true),
                    sectionPrefixAttribute => sectionPrefixAttribute is not null
                        && context.TokenParserCallback.SectionStartsWithPrefix(context, sectionPrefixAttribute.Prefix)
                )
            };
        }

        public static string GetDirectiveName<TState>(this ITemplateSectionProcessor<TState> instance)
            where TState : class
            => instance is IModeledTemplateSectionProcessor<TState> processor
                ? processor.ModelType.GetModelType(typeof(TState)).Name.ToCamelCase().RemoveSuffix("Model").WithoutGenerics()
                : instance.GetType().GetModelType(typeof(TState)).Name.WithoutGenerics();

        public static object GetModel<TState>(this ITemplateSectionProcessor<TState> instance)
            where TState : class
            => instance.GetModelType().Either
            (
                modelType => modelType == typeof(SectionModel),
                _ => new SectionModel(),
                modelType => Activator.CreateInstance(modelType.GetModelType(typeof(TState))).WithDefaultValues()
            );

        public static Type GetModelType<TState>(this ITemplateSectionProcessor<TState> instance)
            where TState : class
            => instance is IModeledTemplateSectionProcessor<TState> processor
                ? processor.ModelType.GetModelType(typeof(TState))
                : instance.GetType().GetCustomAttribute<DirectiveModelAttribute>(true)?.Type?.GetModelType(typeof(TState)) ?? typeof(SectionModel);

        public static string GetDirectivePrefix<TState>(this ITemplateSectionProcessor<TState> instance)
            where TState : class
            => instance is ITemplateCustomDirectiveName name
                ? name.TemplateCustomDirectiveName
                : instance.GetType().GetCustomAttribute<DirectivePrefixAttribute>(true)?.Name ?? instance.GetType().GetCustomAttribute<SectionPrefixAttribute>(true)?.Prefix;

        public static bool IsDirective<TState>(this ITemplateSectionProcessor<TState> instance)
            where TState : class
            => instance.GetType().GetCustomAttribute<DirectiveModelAttribute>(true) is not null
            || instance is IModeledTemplateSectionProcessor<TState>;

        public static IEnumerable<ITemplateSectionProcessor<TState>> GetContainedTemplateSectionProcessors<TState>(this ITemplateSectionProcessor<TState> instance)
            where TState : class
            => instance is ITemplateSectionProcessorContainer<TState> c
                ? c.ContainedTemplateSectionProcessors
                : new[] { instance };
    }
}
