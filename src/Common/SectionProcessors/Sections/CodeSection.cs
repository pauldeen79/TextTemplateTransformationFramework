using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.Common.SectionProcessors.Sections
{
    [Description("Renders code in the Render method, Initialize method, class footer or namespace footer")]
    public sealed class CodeSection<TState> : ICodeSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return SectionProcessResult.Create(new RenderCodeToken<TState>(context, context.Section.Trim()));
        }
    }
}
