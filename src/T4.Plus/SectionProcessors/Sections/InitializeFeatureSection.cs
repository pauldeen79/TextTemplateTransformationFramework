using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.SectionProcessors.Sections
{
    [Description("Renders code on initialization level")]
    [SectionPrefix("*")]
    public sealed class InitializeFeatureSection<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create
            (
                context,
                Mode.CodeInitialize,
                code => new InitializeCodeToken<TState>(context, code)
            );
    }
}
