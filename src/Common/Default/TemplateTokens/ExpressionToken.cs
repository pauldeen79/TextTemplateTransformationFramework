using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public abstract class ExpressionToken<TState> : TemplateToken<TState>, IExpressionToken<TState>
        where TState : class
    {
        protected ExpressionToken(SectionContext<TState> context, string expression)
            : base(context)
        {
            Expression = expression;
        }

        public string Expression { get; }
    }
}
