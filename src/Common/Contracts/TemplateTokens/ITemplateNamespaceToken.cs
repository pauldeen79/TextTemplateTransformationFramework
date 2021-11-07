namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining the namespace of the template.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface ITemplateNamespaceToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Namespace { get; }
    }
}
