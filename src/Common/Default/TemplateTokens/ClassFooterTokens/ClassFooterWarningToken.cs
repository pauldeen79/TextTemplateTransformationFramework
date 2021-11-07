using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens
{
    public class ClassFooterWarningToken<TState> : WarningToken<TState>, IClassFooterWarningToken<TState>
        where TState : class
    {
        public ClassFooterWarningToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
