namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a parameter token.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IParameterToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }
        string TypeName { get; }
        bool NetCoreCompatible { get; }
    }
}
