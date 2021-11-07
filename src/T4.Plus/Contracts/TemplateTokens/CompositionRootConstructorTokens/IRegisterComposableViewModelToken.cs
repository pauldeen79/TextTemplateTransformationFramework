namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.CompositionRootConstructorTokens
{
    public interface IRegisterComposableViewModelToken<TState> : ICompositionRootInitializeToken<TState>
        where TState : class
    {
        string ViewModelName { get; }
        bool ViewModelNameIsLiteral { get; }
        string ViewModelFileName { get; }
        string ModelTypeName { get; }
        bool UseForRouting { get; }
    }
}
