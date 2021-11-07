using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens
{
    public class ClassFooterExpressionToken<TState> : ExpressionToken<TState>, IClassFooterExpressionToken<TState>
        where TState : class
    {
        public ClassFooterExpressionToken(SectionContext<TState> context, string expression)
            : base(context, expression)
        {
        }
    }
}
