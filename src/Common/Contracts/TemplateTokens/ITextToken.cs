namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for rendering text.
    /// </summary>
    public interface ITextToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Contents { get; }
    }
}
