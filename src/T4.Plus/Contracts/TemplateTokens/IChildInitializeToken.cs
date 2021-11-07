using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IChildInitializeToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
