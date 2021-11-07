using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.BaseClassFooterTokens
{
    public class AddChildTemplateCodeToken<TState> : TemplateToken<TState>, IAddChildTemplateCodeToken<TState>
        where TState : class
    {
        public AddChildTemplateCodeToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
