using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TextTemplateProcessorContextExtensions
    {
        public static IEnumerable<ITemplateSectionProcessor<TState>> GetCustomSectionProcessors<TState>(this ITextTemplateProcessorContext<TState> instance)
            where TState : class
            => instance.TryGetValue("Common.CustomSectionProcessors", out var result)
                ? (IEnumerable<ITemplateSectionProcessor<TState>>)result
                : Enumerable.Empty<ITemplateSectionProcessor<TState>>();

        public static ITextTemplateProcessorContext<TState> SetCustomSectionProcessors<TState>(this ITextTemplateProcessorContext<TState> instance, IEnumerable<ITemplateSectionProcessor<TState>> customSectionProcessors)
            where TState : class
        {
            if (customSectionProcessors != null)
            {
                instance["Common.CustomSectionProcessors"] = customSectionProcessors.ToList();
            }

            return instance;
        }
    }
}
