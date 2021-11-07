using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens
{
    public class BaseClassFooterWarningToken<TState> : WarningToken<TState>, IBaseClassFooterWarningToken<TState>
        where TState : class
    {
        public BaseClassFooterWarningToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
