namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for defining a package reference.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IPackageReferenceToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }

        string Version { get; }

        string FrameworkVersion { get; }
    }
}
