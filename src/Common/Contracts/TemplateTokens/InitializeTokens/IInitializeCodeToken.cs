namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens
{
    /// <summary>
    /// Contract for rendering code in the initialize method.
    /// </summary>
    /// <seealso cref="IInitializeToken" />
    /// <seealso cref="ICodeToken" />
    public interface IInitializeCodeToken<TState> : IInitializeToken<TState>, ICodeToken<TState>
        where TState : class
    {
    }
}
