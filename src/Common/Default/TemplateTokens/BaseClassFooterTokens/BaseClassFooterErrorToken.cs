using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens
{
    public class BaseClassFooterErrorToken<TState> : ErrorToken<TState>, IBaseClassFooterErrorToken<TState>
        where TState : class
    {
        public BaseClassFooterErrorToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
