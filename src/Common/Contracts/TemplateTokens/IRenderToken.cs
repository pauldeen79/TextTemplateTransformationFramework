namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Interface for tokens that need to be rendered.
    /// </summary>
    /// <remarks>
    /// This interface has several inherited interfaces, for specific implementations.
    /// </remarks>
    /// <seealso cref="ITemplateToken" />
    public interface IRenderToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
