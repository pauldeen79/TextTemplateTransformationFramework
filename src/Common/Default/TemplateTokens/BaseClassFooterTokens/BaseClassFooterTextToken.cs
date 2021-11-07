using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens
{
    public class BaseClassFooterTextToken<TState> : TextToken<TState>, IBaseClassFooterTextToken<TState>
        where TState : class
    {
        public BaseClassFooterTextToken(SectionContext<TState> context, string contents)
            : base(context, contents)
        {
        }
    }
}
