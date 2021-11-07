using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.CompositionRootFeatureTokens;

namespace TextTemplateTransformationFramework.T4.Plus.SectionProcessors.Sections
{
    [Description("Renders code on composition root class level")]
    [SectionPrefix("&")]
    public sealed class CompositionRootFeatureSection<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create
            (
                context,
                Mode.CodeCompositionRootFeature,
                code => new CompositionRootFeatureCodeToken<TState>(context, code)
            );
    }
}
