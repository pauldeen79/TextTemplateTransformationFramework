namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining an assembly reference.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IReferenceToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }
    }
}
