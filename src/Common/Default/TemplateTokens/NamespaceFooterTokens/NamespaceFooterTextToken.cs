using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens
{
    public class NamespaceFooterTextToken<TState> : TextToken<TState>, INamespaceFooterTextToken<TState>
        where TState : class
    {
        public NamespaceFooterTextToken(SectionContext<TState> context, string contents)
            : base(context, contents)
        {
        }
  }
}
