using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Extensions
{
    public static class SectionProcessResultExtensions
    {
        public static IEnumerable<ITemplateToken<TState>> GetTemplateTokens<TState>(this SectionProcessResult<TState> sectionProcessResult)
            where TState : class
            => sectionProcessResult
                .Tokens
                .Exclude<ITemplateToken<TState>, ISourceSectionToken<TState>>();

        public static IEnumerable<ISourceSectionToken<TState>> GetSourceSectionTokens<TState>(this SectionProcessResult<TState> sectionProcessResult)
            where TState : class
            => sectionProcessResult
                .Tokens
                .OfType<ISourceSectionToken<TState>>();

        public static IEnumerable<ITemplateSectionProcessor<TState>> GetTemplateSectionProcessors<TState>(this SectionProcessResult<TState> sectionProcessResult)
            where TState : class
            => sectionProcessResult
                .Tokens
                .OfType<ITemplateSectionProcessorTemplateToken<TState>>()
                .Select(t => t.TemplateSectionProcessor);
    }
}
