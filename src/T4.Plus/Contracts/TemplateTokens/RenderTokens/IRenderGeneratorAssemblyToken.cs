using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens
{
    public interface IRenderGeneratorAssemblyToken<TState> : IRenderToken<TState>
        where TState : class
    {
        string AssemblyName { get; }
        bool AssemblyNameIsLiteral { get; }
        string BasePath { get; }
        bool BasePathIsLiteral { get; }
        bool GenerateMultipleFiles { get; }
        bool DryRun { get; }
        string CurrentDirectory { get; }
        bool CurrentDirectoryIsLiteral { get; }
    }
}
