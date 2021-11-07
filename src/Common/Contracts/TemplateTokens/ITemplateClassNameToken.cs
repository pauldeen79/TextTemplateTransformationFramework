namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining the class name of the template.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface ITemplateClassNameToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string ClassName { get; }
        string BaseClassName { get; }
    }
}
