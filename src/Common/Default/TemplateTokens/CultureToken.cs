using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class CultureToken<TState> : TemplateToken<TState>, ICultureToken<TState>
        where TState : class
    {
        public CultureToken(SectionContext<TState> context, string code)
            : base(context)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
