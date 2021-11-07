using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class TemplateSectionProcessorTemplateToken<TState> : TemplateToken<TState>, ITemplateSectionProcessorTemplateToken<TState>
        where TState : class
    {
        public TemplateSectionProcessorTemplateToken(SectionContext<TState> context, ITemplateSectionProcessor<TState> templateSectionProcessor)
            : base(context)
        {
            TemplateSectionProcessor = templateSectionProcessor;
        }

        public ITemplateSectionProcessor<TState> TemplateSectionProcessor { get; }
    }
}
