namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining the output extension.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IOutputExtensionToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Extension { get; }
    }
}
