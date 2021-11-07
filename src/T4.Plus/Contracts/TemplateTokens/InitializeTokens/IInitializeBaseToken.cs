using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    public interface IInitializeBaseToken<TState> : IInitializeToken<TState>
        where TState : class
    {
        bool AdditionalActionDelegate { get; }
    }
}
