using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens
{
    public class RenderWarningToken<TState> : WarningToken<TState>, IRenderWarningToken<TState>
        where TState : class
    {
        public RenderWarningToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
