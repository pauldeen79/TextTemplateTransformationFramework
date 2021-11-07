using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    public interface IRegisterPlaceholderToken<TState> : IInitializeToken<TState>, IOrderedItem
        where TState : class
    {
        string ChildTemplateName { get; }
        bool ChildTemplateNameIsLiteral { get; }
        string ModelTypeName { get; }
    }
}
