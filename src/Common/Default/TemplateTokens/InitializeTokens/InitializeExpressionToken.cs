using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens
{
    public class InitializeExpressionToken<TState> : ExpressionToken<TState>, IInitializeExpressionToken<TState>
        where TState : class
    {
        public InitializeExpressionToken(SectionContext<TState> context, string expression)
            : base(context, expression)
        {
        }
    }
}
