using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens
{
    public interface IRenderErrorToken<TState> : IRenderToken<TState>, IErrorToken<TState>
        where TState : class
    {
    }
}
