using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens
{
    public class NamespaceFooterExpressionToken<TState> : ExpressionToken<TState>, INamespaceFooterExpressionToken<TState>
        where TState : class
    {
        public NamespaceFooterExpressionToken(SectionContext<TState> context, string expression)
            : base(context, expression)
        {
        }
    }
}
