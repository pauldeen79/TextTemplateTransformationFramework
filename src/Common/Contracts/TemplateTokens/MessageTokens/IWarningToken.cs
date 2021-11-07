namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens
{
    public interface IWarningToken<TState> : IMessageToken<TState>
        where TState : class
    {
    }
}
