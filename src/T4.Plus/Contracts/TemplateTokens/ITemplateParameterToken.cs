using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a template parameter.
    /// </summary>
    public interface ITemplateParameterToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }
        string Value { get; }
    }
}
