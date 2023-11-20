using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.SectionProcessors.Sections
{
    [Description("Renders an expression in the Render method, class footer or namespace footer")]
    [SectionPrefix("=")]
    public sealed class ExpressionSection<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context
                .CreateExpressionToken(context.Section.Substring(1).Trim())
                .Either
                (
                    t => SectionProcessResult.Create(t),
                    () => SectionProcessResult<TState>.NotUnderstood
                );
        }
    }
}
