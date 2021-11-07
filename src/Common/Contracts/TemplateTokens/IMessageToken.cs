namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a render token which contains a message.
    /// </summary>
    public interface IMessageToken<TState> : ITemplateToken<TState>
        where TState: class
    {
        string Message { get; }
    }
}
