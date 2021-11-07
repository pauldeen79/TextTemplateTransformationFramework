using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ICopyPropertiesToViewModelToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        bool Enabled { get; }
    }
}
