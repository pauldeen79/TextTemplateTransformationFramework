using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens
{
    public abstract class WarningToken<TState> : TemplateToken<TState>, IWarningToken<TState>
        where TState : class
    {
        protected WarningToken(SectionContext<TState> context, string message)
            : base(context)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
