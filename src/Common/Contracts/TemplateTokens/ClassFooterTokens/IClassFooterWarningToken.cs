using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens
{
    public interface IClassFooterWarningToken<TState> : IClassFooterToken<TState>, IWarningToken<TState>
        where TState : class
    {
    }
}
