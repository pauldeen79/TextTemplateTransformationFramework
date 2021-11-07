using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens
{
    public abstract class ErrorToken<TState> : TemplateToken<TState>, IErrorToken<TState>
        where TState : class
    {
        protected ErrorToken(SectionContext<TState> context, string message)
            : base(context)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
