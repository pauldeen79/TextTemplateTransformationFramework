namespace TemplateFramework.Core.CodeGeneration;

[ExcludeFromCodeCoverage]
public sealed class CodeGenerationProviderWrapper : ICodeGenerationProvider
{
    private readonly object _instance;

    public CodeGenerationProviderWrapper(object instance) => _instance = instance;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8605 // Unboxing a possibly null value.
    public bool GenerateMultipleFiles => (bool)_instance.GetType().GetProperty(nameof(GenerateMultipleFiles)).GetValue(_instance);

    public bool SkipWhenFileExists => (bool)_instance.GetType().GetProperty(nameof(SkipWhenFileExists)).GetValue(_instance);

    public string BasePath => (string)_instance.GetType().GetProperty(nameof(BasePath)).GetValue(_instance);

    public string Path => (string)_instance.GetType().GetProperty(nameof(Path)).GetValue(_instance);

    public string DefaultFilename => (string)_instance.GetType().GetProperty(nameof(DefaultFilename)).GetValue(_instance);

    public bool RecurseOnDeleteGeneratedFiles => (bool)_instance.GetType().GetProperty(nameof(RecurseOnDeleteGeneratedFiles)).GetValue(_instance);

    public string LastGeneratedFilesFilename => (string)_instance.GetType().GetProperty(nameof(LastGeneratedFilesFilename)).GetValue(_instance);

    public object? CreateAdditionalParameters() => _instance.GetType().GetMethod(nameof(CreateAdditionalParameters)).Invoke(_instance, Array.Empty<object>());

    public object CreateGenerator() => _instance.GetType().GetMethod(nameof(CreateGenerator)).Invoke(_instance, Array.Empty<object>());

    public object? CreateModel() => _instance.GetType().GetMethod(nameof(CreateModel)).Invoke(_instance, Array.Empty<object>());

    public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath) => _instance.GetType().GetMethod(nameof(Initialize)).Invoke(_instance, new object[] { generateMultipleFiles, skipWhenFileExists, basePath });
#pragma warning restore CS8605 // Unboxing a possibly null value.
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}
