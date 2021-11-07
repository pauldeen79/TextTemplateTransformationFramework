using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IPreLoadToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }
    }
}
