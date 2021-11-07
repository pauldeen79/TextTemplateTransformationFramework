using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ILogToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Message { get; }
    }
}
