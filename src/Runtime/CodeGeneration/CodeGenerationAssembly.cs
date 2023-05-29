using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace TextTemplateTransformationFramework.Runtime.CodeGeneration
{
    public sealed class CodeGenerationAssembly : ICodeGenerationAssembly, IDisposable
    {
        private readonly string _assemblyName;
        private readonly string _basePath;
        private readonly bool _generateMultipleFiles;
        private readonly bool _dryRun;
        private readonly IEnumerable<string> _classNameFilter;
        private readonly AssemblyLoadContext _context;

        public CodeGenerationAssembly(string assemblyName,
                                      string basePath,
                                      bool generateMultipleFiles,
                                      bool dryRun,
                                      string currentDirectory = null,
                                      IEnumerable<string> classNameFilter = null)
        {
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));
            if (basePath == null) throw new ArgumentNullException(nameof(basePath));
            _assemblyName = assemblyName;
            _basePath = basePath;
            _generateMultipleFiles = generateMultipleFiles;
            _dryRun = dryRun;
            _classNameFilter = classNameFilter ?? Enumerable.Empty<string>();
#if !NETFRAMEWORK
            if (string.IsNullOrEmpty(currentDirectory))
            {
                currentDirectory = Directory.GetCurrentDirectory();
            }
            _context = new CustomAssemblyLoadContext("T4PlusCmd", true, () => new[] { currentDirectory });
#endif
        }

        public string Generate()
        {

            var assembly = LoadAssembly(_context ?? AssemblyLoadContext.Default);
            var settings = new CodeGenerationSettings(_basePath, _generateMultipleFiles, _dryRun);
            return GetOutputFromAssembly(assembly, settings);
        }

        public void Dispose()
        {
#if !NETFRAMEWORK
            _context?.Unload();
#endif
        }

        private Assembly LoadAssembly(AssemblyLoadContext context)
        {
            try
            {
                return context.LoadFromAssemblyName(new AssemblyName(_assemblyName));
            }
            catch (FileLoadException fle) when (fle.Message.StartsWith("The given assembly name was invalid."))
            {
                return context.LoadFromAssemblyPath(_assemblyName);
            }
        }

        private string GetOutputFromAssembly(Assembly assembly, CodeGenerationSettings settings)
        {
            var multipleContentBuilder = new MultipleContentBuilder { BasePath = settings.BasePath };
            foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly))
            {
                GenerateCode.For(settings, multipleContentBuilder, codeGenerationProvider);
            }

            return multipleContentBuilder.ToString();
        }

        private IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly)
            => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces().Any(i => i.FullName == "TextTemplateTransformationFramework.Runtime.CodeGeneration.ICodeGenerationProvider"))
                .Where(t => FilterIsValid(t))
                .Select(t => new CodeGenerationProviderWrapper(Activator.CreateInstance(t)));

        private bool FilterIsValid(Type type)
            => !_classNameFilter.Any() || _classNameFilter.Any(x => x == type.FullName);
    }
}
