using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TokenParserCallbackExtensions
    {
        public static string GetSectionArgument<TState>(this ITokenParserCallback<TState> instance, SectionContext<TState> context, string name)
            where TState : class
            => instance.GetSectionArguments(context, name).FirstOrDefault();

        /// <summary>
        /// Gets the custom section processors.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ITemplateSectionProcessor<TState>> GetCustomSectionProcessors<TState>(this ITokenParserCallback<TState> instance)
            where TState : class
            => instance.ContainsKey("Common.CustomSectionProcessors")
                ? instance.GetValue<ICollection<ITemplateSectionProcessor<TState>>>("Common.CustomSectionProcessors")
                : Enumerable.Empty<ITemplateSectionProcessor<TState>>();

        /// <summary>
        /// Sets the custom section processors.
        /// </summary>
        /// <param name="customSectionProcessors">Custom section processors to use.</param>
        public static ITokenParserCallback<TState> SetCustomSectionProcessors<TState>(this ITokenParserCallback<TState> instance, IEnumerable<ITemplateSectionProcessor<TState>> customSectionProcessors)
            where TState : class
        {
            if (instance.ContainsKey("Common.CustomSectionProcessors"))
            {
                instance.Remove("Common.CustomSectionProcessors");
            }

            if (customSectionProcessors is not null)
            {
                instance["Common.CustomSectionProcessors"] = customSectionProcessors.ToList();
            }

            return instance;
        }
    }
}
