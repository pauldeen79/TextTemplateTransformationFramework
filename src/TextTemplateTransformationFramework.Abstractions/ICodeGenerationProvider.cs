namespace TemplateFramework.Abstractions;

public interface ICodeGenerationProvider
{
    bool GenerateMultipleFiles { get; }
    bool SkipWhenFileExists { get; }
    string BasePath { get; }
    string Path { get; }
    string DefaultFilename { get; }
    bool RecurseOnDeleteGeneratedFiles { get; }
    string LastGeneratedFilesFilename { get; }

    void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath);
    object CreateGenerator();
    object CreateAdditionalParameters();
}

public interface ICodeGenerationProvider<out T> : ICodeGenerationProvider
{
    T CreateModel();
}
