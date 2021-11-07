namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining the culture of the template.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface ICultureToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Code { get; }
    }
}
