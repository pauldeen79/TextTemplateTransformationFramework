using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens
{
    public interface IInitializeErrorToken<TState> : IInitializeToken<TState>, IErrorToken<TState>
        where TState : class
    {
    }
}
