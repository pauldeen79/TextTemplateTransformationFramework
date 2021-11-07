namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens
{
    public interface IErrorToken<TState> : IMessageToken<TState>
        where TState : class
    {
    }
}
