using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens
{
    /// <summary>
    /// Contract for registering a child template.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IRegisterChildTemplateToken<TState> : IInitializeToken<TState>
        where TState : class
    {
        string ChildTemplateName { get; }
        bool ChildTemplateNameIsLiteral { get; }
        string ChildTemplateFileName { get; }
        string ModelTypeName { get; }
        bool UseForRouting { get; }
    }
}
