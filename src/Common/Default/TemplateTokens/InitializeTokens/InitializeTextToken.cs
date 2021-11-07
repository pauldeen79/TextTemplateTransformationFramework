using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens
{
    public class InitializeTextToken<TState> : TextToken<TState>, IInitializeTextToken<TState>
        where TState : class
    {
        public InitializeTextToken(SectionContext<TState> context, string contents)
            : base(context, contents)
        {
        }
    }
}
