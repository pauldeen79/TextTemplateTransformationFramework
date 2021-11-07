using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.Tokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens
{
    public class InitializeWarningToken<TState> : WarningToken<TState>, IInitializeWarningToken<TState>
        where TState : class
    {
        public InitializeWarningToken(SectionContext<TState> context, string message)
            : base(context, message)
        {
        }
    }
}
