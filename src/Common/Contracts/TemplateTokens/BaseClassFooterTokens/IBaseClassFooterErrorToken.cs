using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens
{
    public interface IBaseClassFooterErrorToken<TState> : IBaseClassFooterToken<TState>, IErrorToken<TState>
        where TState : class
    {
    }
}
