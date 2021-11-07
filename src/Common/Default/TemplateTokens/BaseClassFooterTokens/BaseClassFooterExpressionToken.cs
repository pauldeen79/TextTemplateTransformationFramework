using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens
{
    public class BaseClassFooterExpressionToken<TState> : ExpressionToken<TState>, IBaseClassFooterExpressionToken<TState>
        where TState : class
    {
        public BaseClassFooterExpressionToken(SectionContext<TState> context, string expression)
            : base(context, expression)
        {
        }
    }
}
