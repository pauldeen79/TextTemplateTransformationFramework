using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens
{
    public class NamespaceFooterWarningToken<TState> : WarningToken<TState>, INamespaceFooterWarningToken<TState>
        where TState : class
    {
        public NamespaceFooterWarningToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
