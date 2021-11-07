namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining the language of the template.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface ILanguageToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        Language Value { get; }

        string SourceValue { get; }
    }
}
