using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.BaseClassFooterTokens
{
    public class AddComposableChildTemplateCodeToken<TState> : TemplateToken<TState>, IAddComposableChildTemplateCodeToken<TState>
        where TState : class
    {
        public AddComposableChildTemplateCodeToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
