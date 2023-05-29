using System;
using System.Diagnostics.CodeAnalysis;

namespace TextTemplateTransformationFramework.Runtime.CodeGeneration
{
    [ExcludeFromCodeCoverage]
    public sealed class CodeGenerationProviderWrapper : ICodeGenerationProvider
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
