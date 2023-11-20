using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.SectionProcessors.Sections
{
    [Description("Renders text")]
    [SectionPrefix("")]
    public sealed class TextSection<TState> : ITextSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context
                .CreateTextToken(context.Section, false)
                .Either
                (
                    token => SectionProcessResult.Create(token),
                    () => SectionProcessResult.Create(new InitializeErrorToken<TState>(context, "Unsupported mode: " + context.CurrentMode))
                );
        }
    }
}
