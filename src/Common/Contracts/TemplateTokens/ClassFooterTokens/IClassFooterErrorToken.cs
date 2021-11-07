using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens
{
    public interface IClassFooterErrorToken<TState> : IClassFooterToken<TState>, IErrorToken<TState>
        where TState : class
    {
    }
}
