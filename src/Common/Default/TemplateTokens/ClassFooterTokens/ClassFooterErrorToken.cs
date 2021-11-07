using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens
{
    public class ClassFooterErrorToken<TState> : ErrorToken<TState>, IClassFooterErrorToken<TState>
        where TState : class
    {
        public ClassFooterErrorToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
