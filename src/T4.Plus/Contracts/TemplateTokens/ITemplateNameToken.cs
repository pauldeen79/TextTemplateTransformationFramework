using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ITemplateNameToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string TemplateName { get; }
    }
}
