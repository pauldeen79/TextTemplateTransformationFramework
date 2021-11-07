using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens
{
    public class ClassFooterTextToken<TState> : TextToken<TState>, IClassFooterTextToken<TState>
        where TState : class
    {
        public ClassFooterTextToken(SectionContext<TState> context, string contents)
            : base(context, contents)
        {
        }
    }
}
