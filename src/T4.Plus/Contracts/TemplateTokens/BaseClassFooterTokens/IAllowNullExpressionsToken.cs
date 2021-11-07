using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.BaseClassFooterTokens
{
    public interface IAllowNullExpressionsToken<TState> : IBaseClassFooterToken<TState>
        where TState : class
    {
        string TemplateClassName { get; }
    }
}
