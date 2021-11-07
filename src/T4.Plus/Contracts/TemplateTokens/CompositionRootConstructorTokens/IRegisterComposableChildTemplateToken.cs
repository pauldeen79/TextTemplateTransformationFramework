namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.CompositionRootConstructorTokens
{
    public interface IRegisterComposableChildTemplateToken<TState> : ICompositionRootInitializeToken<TState>
        where TState : class
    {
        string ChildTemplateName { get; }
        bool ChildTemplateNameIsLiteral { get; }
        string ChildTemplateFileName { get; }
        string ModelTypeName { get; }
        bool UseForRouting { get; }
    }
}
