using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.SectionProcessors.Sections
{
    [Description("Renders code on base class feature level")]
    [SectionPrefix("%")]
    public sealed class BaseClassFeatureSection<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create
            (
                context,
                Mode.CodeBaseClassFeature,
                code => new BaseClassFooterCodeToken<TState>(context, code)
            );
    }
}
