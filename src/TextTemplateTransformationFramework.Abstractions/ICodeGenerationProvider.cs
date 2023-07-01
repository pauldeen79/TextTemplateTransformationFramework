namespace TextTemplateTransformationFramework.Abstractions;

public interface ICodeGenerationProvider
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
    object CreateAdditionalParameters();
}

public interface ICodeGenerationProvider<out T> : ICodeGenerationProvider
{
    T CreateModel();
}
