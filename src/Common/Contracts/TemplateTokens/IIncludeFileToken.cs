namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining an include file.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IIncludeFileToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string IncludeFileName { get; }
    }
}
