using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens
{
    public class RenderErrorToken<TState> : ErrorToken<TState>, IRenderErrorToken<TState>
        where TState : class
    {
        public RenderErrorToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
