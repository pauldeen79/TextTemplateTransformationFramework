using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens
{
    public class BaseClassFooterCodeToken<TState> : CodeToken<TState>, IBaseClassFooterCodeToken<TState>
        where TState : class
    {
        public BaseClassFooterCodeToken(SectionContext<TState> context, string code)
            : base(context, code)
        {
        }
    }
}
