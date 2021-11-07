using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens
{
    public class ClassFooterCodeToken<TState> : CodeToken<TState>, IClassFooterCodeToken<TState>
        where TState : class
    {
        public ClassFooterCodeToken(SectionContext<TState> context, string code)
            : base(context, code)
        {
        }
    }
}
