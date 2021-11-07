using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ITemplateBasePathToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string BasePath { get; }
    }
}
