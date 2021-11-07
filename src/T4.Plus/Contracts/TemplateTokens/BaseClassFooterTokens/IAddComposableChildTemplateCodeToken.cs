using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.BaseClassFooterTokens
{
    public interface IAddComposableChildTemplateCodeToken<TState> : IBaseClassFooterToken<TState>
        where TState : class
    {
    }
}
