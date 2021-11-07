using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens
{
    public class RenderTextToken<TState> : TextToken<TState>, IRenderTextToken<TState>
        where TState : class
    {
        public RenderTextToken(SectionContext<TState> context, string contents)
            : base(context, contents)
        {
        }
    }
}
