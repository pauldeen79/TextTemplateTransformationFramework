using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    public interface IRegisterViewModelToken<TState> : IInitializeToken<TState>
        where TState : class
    {
        string ViewModelName { get; }
        bool ViewModelNameIsLiteral { get; }
        string ViewModelFileName { get; }
        string ModelTypeName { get; }
        bool UseForRouting { get; }
    }
}
