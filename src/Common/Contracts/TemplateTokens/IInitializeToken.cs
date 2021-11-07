namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a token that needs to be rendered in the Initialize method of the template.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IInitializeToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
