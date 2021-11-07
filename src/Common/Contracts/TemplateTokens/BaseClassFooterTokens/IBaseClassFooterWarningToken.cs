using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens
{
    public interface IBaseClassFooterWarningToken<TState> : IBaseClassFooterToken<TState>, IWarningToken<TState>
        where TState : class
    {
    }
}
