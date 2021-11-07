using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ICompositionRootFeatureToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
