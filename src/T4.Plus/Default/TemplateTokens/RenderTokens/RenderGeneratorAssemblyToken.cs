using System;
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
            ValueSpecifier assemblyName,
            ValueSpecifier basePath,
            ValueSpecifier currentDirectory,
            bool generateMultipleFiles,
            bool dryRun)
            : base(context)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            if (basePath == null)
            {
                throw new ArgumentNullException(nameof(basePath));
            }

            if (currentDirectory == null)
            {
                throw new ArgumentNullException(nameof(currentDirectory));
            }

            AssemblyName = assemblyName.Value;
            AssemblyNameIsLiteral = assemblyName.ValueIsLiteral;
            BasePath = basePath.Value;
            BasePathIsLiteral = basePath.ValueIsLiteral;
            GenerateMultipleFiles = generateMultipleFiles;
            DryRun = dryRun;
            CurrentDirectory = currentDirectory.Value;
            CurrentDirectoryIsLiteral = currentDirectory.ValueIsLiteral;
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
