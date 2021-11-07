using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public abstract class CodeToken<TState> : TemplateToken<TState>, ICodeToken<TState>
        where TState : class
    {
        protected CodeToken(SectionContext<TState> context, string code)
            : base(context)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
