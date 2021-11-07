using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.CompositionRootConstructorTokens;

namespace TextTemplateTransformationFramework.T4.Plus.SectionProcessors.Sections
{
    [Description("Renders code on composition root initialize level")]
    [SectionPrefix("$")]
    public sealed class CompositionRootInitializeFeatureSection<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create
            (
                context,
                Mode.CodeCompositionRootInitialize,
                code => new CompositionRootInitializeCodeToken<TState>(context, code)
            );
    }
}
