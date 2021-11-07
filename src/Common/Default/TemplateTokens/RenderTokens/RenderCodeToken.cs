using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens
{
    public class RenderCodeToken<TState> : CodeToken<TState>, IRenderCodeToken<TState>
        where TState : class
    {
        public RenderCodeToken(SectionContext<TState> context, string code)
            : base(context, code)
        {
        }
    }
}
