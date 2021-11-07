using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens
{
    public class RenderExpressionToken<TState> : ExpressionToken<TState>, IRenderExpressionToken<TState>
        where TState : class
    {
        public RenderExpressionToken(SectionContext<TState> context, string expression)
            : base(context, expression)
        {
        }
    }
}
