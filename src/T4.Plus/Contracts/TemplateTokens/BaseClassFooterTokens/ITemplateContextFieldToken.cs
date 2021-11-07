using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.BaseClassFooterTokens
{
    public interface ITemplateContextFieldToken<TState> : IBaseClassFooterToken<TState>
        where TState : class
    {
    }
}
