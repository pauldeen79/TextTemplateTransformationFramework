using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens
{
    public class InitializeErrorToken<TState> : ErrorToken<TState>, IInitializeErrorToken<TState>
        where TState : class
    {
        public InitializeErrorToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
