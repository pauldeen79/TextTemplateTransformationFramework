using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    public interface IInitializeViewModelToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string ViewModelName { get; }
        bool ViewModelNameIsLiteral { get; }
        bool CustomResolverDelegateExpressionIsLiteral { get; }
        string ResolverDelegateModel { get; }
        bool ResolverDelegateModelIsLiteral { get; }
        bool AddRootTemplatePrefix { get; }
        string Model { get; }
        bool ModelIsLiteral { get; }
        bool SilentlyContinueOnError { get; }
        string CustomResolverDelegateExpression { get; }
    }
}
