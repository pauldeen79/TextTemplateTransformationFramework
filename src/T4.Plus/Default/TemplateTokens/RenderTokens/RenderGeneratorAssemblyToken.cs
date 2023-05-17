using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens
{
    public class RenderGeneratorAssemblyToken<TState> : TemplateToken<TState>, IRenderGeneratorAssemblyToken<TState>
        where TState : class
    {
        public RenderGeneratorAssemblyToken(
            SectionContext<TState> context,
            string assemblyName,
            bool assemblyNameIsLiteral,
            string basePath,
            bool basePathIsLiteral,
            bool generateMultipleFiles,
            bool dryRun,
            string currentDirectory,
            bool currentDirectoryIsLiteral)
            : base(context)
        {
            AssemblyName = assemblyName;
            AssemblyNameIsLiteral = assemblyNameIsLiteral;
            BasePath = basePath;
            BasePathIsLiteral = basePathIsLiteral;
            GenerateMultipleFiles = generateMultipleFiles;
            DryRun = dryRun;
            CurrentDirectory = currentDirectory;
            CurrentDirectoryIsLiteral = currentDirectoryIsLiteral;
        }

        public string AssemblyName { get; }
        public bool AssemblyNameIsLiteral { get; }
        public string BasePath { get; }
        public bool BasePathIsLiteral { get; }
        public bool GenerateMultipleFiles { get; }
        public bool DryRun { get; }
        public string CurrentDirectory { get; }
        public bool CurrentDirectoryIsLiteral { get; }
    }
}
