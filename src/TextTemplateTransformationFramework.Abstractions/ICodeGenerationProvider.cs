namespace TextTemplateTransformationFramework.Abstractions;

public interface ICodeGenerationProvider : ICodeGenerationProvider<object?>
{
}

public interface ICodeGenerationProvider<out T>
{
    bool GenerateMultipleFiles { get; }
    bool SkipWhenFileExists { get; }
    string BasePath { get; }
    string Path { get; }
    string DefaultFileName { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFileName { get; }

    void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath);
    object CreateGenerator();
    T? CreateModel();
    object CreateAdditionalParameters();
}
