using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens
{
    public class NamespaceFooterErrorToken<TState> : ErrorToken<TState>, INamespaceFooterErrorToken<TState>
        where TState : class
    {
        public NamespaceFooterErrorToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
