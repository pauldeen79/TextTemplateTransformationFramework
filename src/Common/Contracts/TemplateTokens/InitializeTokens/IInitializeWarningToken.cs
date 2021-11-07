using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens
{
    public interface IInitializeWarningToken<TState> : IInitializeToken<TState>, IWarningToken<TState>
        where TState : class
    {
    }
}
