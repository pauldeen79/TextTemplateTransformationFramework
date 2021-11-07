using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    public interface IClearErrorsToken<TState> : IInitializeToken<TState>, IOrderedItem
        where TState : class
    {
    }
}
