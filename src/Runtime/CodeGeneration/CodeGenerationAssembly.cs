#if !NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        [ExcludeFromCodeCoverage]
        private sealed class CodeGenerationProviderWrapper : ICodeGenerationProvider
        {
            private readonly object _instance;

            public CodeGenerationProviderWrapper(object instance)
            {
                _instance = instance;
            }

            public bool GenerateMultipleFiles => (bool)_instance.GetType().GetProperty(nameof(GenerateMultipleFiles)).GetValue(_instance);

            public bool SkipWhenFileExists => (bool)_instance.GetType().GetProperty(nameof(SkipWhenFileExists)).GetValue(_instance);

            public string BasePath => (string)_instance.GetType().GetProperty(nameof(BasePath)).GetValue(_instance);

            public string Path => (string)_instance.GetType().GetProperty(nameof(Path)).GetValue(_instance);

            public string DefaultFileName => (string)_instance.GetType().GetProperty(nameof(DefaultFileName)).GetValue(_instance);

            public bool RecurseOnDeleteGeneratedFiles => (bool)_instance.GetType().GetProperty(nameof(RecurseOnDeleteGeneratedFiles)).GetValue(_instance);

            public string LastGeneratedFilesFileName => (string)_instance.GetType().GetProperty(nameof(LastGeneratedFilesFileName)).GetValue(_instance);

            public Action AdditionalActionDelegate => (Action)_instance.GetType().GetProperty(nameof(AdditionalActionDelegate)).GetValue(_instance);

            public object CreateAdditionalParameters() => _instance.GetType().GetMethod(nameof(CreateAdditionalParameters)).Invoke(_instance, Array.Empty<object>());

            public object CreateGenerator() => _instance.GetType().GetMethod(nameof(CreateGenerator)).Invoke(_instance, Array.Empty<object>());

            public object CreateModel() => _instance.GetType().GetMethod(nameof(CreateModel)).Invoke(_instance, Array.Empty<object>());

            public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath) => _instance.GetType().GetMethod(nameof(Initialize)).Invoke(_instance, new object[] { generateMultipleFiles, skipWhenFileExists, basePath });
        }
    }
}
#endif
