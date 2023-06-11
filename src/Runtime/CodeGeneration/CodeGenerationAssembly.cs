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
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S3459 // Unassigned members should be removed
#pragma warning disable CS0649
        private readonly AssemblyLoadContext _context;
        private readonly string _currentDirectory;
#pragma warning restore CS0649
#pragma warning restore S3459 // Unassigned members should be removed
#pragma warning restore IDE0079 // Remove unnecessary suppression

        public CodeGenerationAssembly(string assemblyName,
                                      string basePath,
                                      bool generateMultipleFiles,
                                      bool dryRun,
                                      string currentDirectory = null,
                                      IEnumerable<string> classNameFilter = null)
        {
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));
            _assemblyName = assemblyName;
            _basePath = basePath;
            _generateMultipleFiles = generateMultipleFiles;
            _dryRun = dryRun;
            _classNameFilter = classNameFilter ?? Enumerable.Empty<string>();
#if !NETFRAMEWORK
            if (string.IsNullOrEmpty(currentDirectory))
            {
                _currentDirectory = Directory.GetCurrentDirectory();
            }
            else
            {
                _currentDirectory = currentDirectory;
            }
            _context = new CustomAssemblyLoadContext("T4PlusCmd", true, () => new[] { _currentDirectory });
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
            catch (Exception e) when (e.Message.StartsWith("The given assembly name was invalid.") || e.Message.EndsWith("The system cannot find the file specified."))
            {
                var assemblyName = _assemblyName;
                if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathRooted(assemblyName))
                {
                    assemblyName = Path.Combine(!string.IsNullOrEmpty(_currentDirectory) ? _currentDirectory : Directory.GetCurrentDirectory(), assemblyName);
                }
                return context.LoadFromAssemblyPath(assemblyName);
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
            => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && Array.Exists(t.GetInterfaces(), i => i.FullName == typeof(ICodeGenerationProvider).FullName))
                .Where(FilterIsValid)
                .Select(t => new CodeGenerationProviderWrapper(Activator.CreateInstance(t)));

        private bool FilterIsValid(Type type)
            => !_classNameFilter.Any() || _classNameFilter.Any(x => x == type.FullName);
    }
}
