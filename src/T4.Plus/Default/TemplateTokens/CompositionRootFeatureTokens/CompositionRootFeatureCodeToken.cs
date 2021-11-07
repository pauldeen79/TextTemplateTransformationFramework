using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.CompositionRootFeatureTokens
{
    public class CompositionRootFeatureCodeToken<TState> : TemplateToken<TState>, ICompositionRootFeatureToken<TState>, ICodeToken<TState>
        where TState : class
    {
        public CompositionRootFeatureCodeToken(SectionContext<TState> context, string code)
            : base(context)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
