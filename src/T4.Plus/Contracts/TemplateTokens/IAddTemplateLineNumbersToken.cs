using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IAddTemplateLineNumbersToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        bool Enabled { get; }
    }
}
