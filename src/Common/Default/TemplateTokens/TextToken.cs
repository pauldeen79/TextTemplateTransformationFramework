using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public abstract class TextToken<TState> : TemplateToken<TState>, ITextToken<TState>
        where TState : class
    {
        protected TextToken(SectionContext<TState> context, string contents)
            : base(context)
        {
            Contents = contents;
        }

        public string Contents { get; }
    }
}
