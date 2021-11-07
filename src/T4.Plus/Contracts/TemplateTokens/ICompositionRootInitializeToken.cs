using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ICompositionRootInitializeToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
