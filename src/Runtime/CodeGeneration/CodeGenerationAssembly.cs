#if !NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TextTemplateTransformationFramework.Runtime.CodeGeneration
{
    public sealed class CodeGenerationAssembly : IDisposable
    {
        private readonly string _assemblyName;
        private readonly string _basePath;
        private readonly bool _generateMultipleFiles;
        private readonly bool _dryRun;
        private readonly CustomAssemblyLoadContext _context;

        public void Dispose() => _context.Unload();

        public CodeGenerationAssembly(string assemblyName, string basePath, bool generateMultipleFiles, bool dryRun, string currentDirectory = null)
        {
            _assemblyName = assemblyName;
            _basePath = basePath;
            _generateMultipleFiles = generateMultipleFiles;
            _dryRun = dryRun;
            if (string.IsNullOrEmpty(currentDirectory))
            {
                currentDirectory = Directory.GetCurrentDirectory();
            }
            _context = new CustomAssemblyLoadContext("T4PlusCmd", true, () => new[] { currentDirectory });
        }

        public string Generate()
        {
            var assembly = LoadAssembly(_assemblyName);
            var settings = new CodeGenerationSettings(_basePath, _generateMultipleFiles, _dryRun);
            return GetOutputFromAssembly(assembly, settings);
        }

        private Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return _context.LoadFromAssemblyName(new AssemblyName(assemblyName));
            }
            catch (FileLoadException fle) when (fle.Message.StartsWith("The given assembly name was invalid."))
            {
                return _context.LoadFromAssemblyPath(assemblyName);
            }
        }

        private static string GetOutputFromAssembly(Assembly assembly, CodeGenerationSettings settings)
        {
            var multipleContentBuilder = new MultipleContentBuilder { BasePath = settings.BasePath };
            foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly))
            {
                GenerateCode.For(settings, multipleContentBuilder, codeGenerationProvider);
            }

            return multipleContentBuilder.ToString();
        }

        private static IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly)
            => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces().Any(i => i.FullName == "TextTemplateTransformationFramework.Runtime.CodeGeneration.ICodeGenerationProvider"))
                .Select(t => new CodeGenerationProviderWrapper(Activator.CreateInstance(t)));
    }
}
#endif
