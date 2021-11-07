using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IAddTemplateToPlaceholderToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string PlaceholderName { get; }
        bool PlaceholderNameIsLiteral { get; }
        string ChildTemplateName { get; }
        bool ChildTemplateNameIsLiteral { get; }
    }
}
