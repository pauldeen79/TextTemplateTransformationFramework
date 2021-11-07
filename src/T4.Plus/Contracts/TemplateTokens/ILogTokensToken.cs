using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ILogTokensToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
