using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IAddExcludeFromCodeCoverageAttributesToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        bool Enabled { get; }
    }
}
