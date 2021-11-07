namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining a custom base class for a template.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IBaseClassToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string BaseClassName { get; }
    }
}
