using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens;

namespace TextTemplateTransformationFramework.Common.SectionProcessors.Sections
{
    [Description("Renders code on class level")]
    [SectionPrefix("+")]
    public sealed class ClassFeatureSection<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create
            (
                context,
                Mode.CodeClassFeature,
                code => new ClassFooterCodeToken<TState>(context, code)
            );
    }
}
