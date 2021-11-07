using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    public interface IClearGenerationEnvironmentToken<TState> : IInitializeToken<TState>
        where TState : class
    {
    }
}
