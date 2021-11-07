using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.SectionProcessors.Sections
{
    [Description("Renders code on namespace level")]
    [SectionPrefix("^")]
    public sealed class NamespaceFeatureSection<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create
            (
                context,
                Mode.CodeNamespaceFeature,
                code => new NamespaceFooterCodeToken<TState>(context, code)
            );
    }
}
