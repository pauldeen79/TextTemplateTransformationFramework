using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens
{
    public class RenderGeneratorAssemblyToken<TState> : TemplateToken<TState>, IRenderGeneratorAssemblyToken<TState>
        where TState : class
    {
        public RenderGeneratorAssemblyToken(SectionContext<TState> context, string assemblyName, string basePath, bool generateMultipleFiles, bool dryRun, string currentDirectory)
            : base(context)
        {
            AssemblyName = assemblyName;
            BasePath = basePath;
            GenerateMultipleFiles = generateMultipleFiles;
            DryRun = dryRun;
            CurrentDirectory = currentDirectory;
        }

        public string AssemblyName { get; }
        public string BasePath { get; }
        public bool GenerateMultipleFiles { get; }
        public bool DryRun { get; }
        public string CurrentDirectory { get; }
    }
}
