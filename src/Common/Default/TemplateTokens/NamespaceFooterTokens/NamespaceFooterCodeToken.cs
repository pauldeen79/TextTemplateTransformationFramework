using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens
{
    public class NamespaceFooterCodeToken<TState> : CodeToken<TState>, INamespaceFooterCodeToken<TState>
        where TState : class
    {
        public NamespaceFooterCodeToken(SectionContext<TState> context, string code)
            : base(context, code)
        {
        }
    }
}
