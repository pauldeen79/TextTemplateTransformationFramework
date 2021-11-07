using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    public interface ICallAdditionalActionDelegateToken<TState> : IInitializeToken<TState>
        where TState : class
    {
        bool SkipInitializationCode { get; }
    }
}
