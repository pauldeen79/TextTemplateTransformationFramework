using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens
{
    public interface IRenderWarningToken<TState> : IRenderToken<TState>, IWarningToken<TState>
        where TState : class
    {
    }
}
