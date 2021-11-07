using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens
{
    public class InitializeCodeToken<TState> : CodeToken<TState>, IInitializeCodeToken<TState>
        where TState : class
    {
        public InitializeCodeToken(SectionContext<TState> context, string code)
            : base(context, code)
        {
        }
    }
}
