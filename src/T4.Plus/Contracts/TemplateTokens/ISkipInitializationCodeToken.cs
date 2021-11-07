using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ISkipInitializationCodeToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
