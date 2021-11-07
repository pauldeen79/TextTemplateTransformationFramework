using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IHintPathToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }
        string HintPath { get; }
        bool Recursive { get; }
    }
}
