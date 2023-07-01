namespace TextTemplateTransformationFramework.Core;

internal static class CodeGenerationEngineBase
{
    internal static (object generator, bool shouldSave, bool shouldUseLastGeneratedFiles, object additionalParameters, ITemplateFileManager templateFileManager) Initialize(ICodeGenerationProvider provider, ICodeGenerationSettings settings, ITemplateFileManagerFactory templateFileManagerFactory, string basePath)
    {
        Guard.IsNotNull(settings);
        Guard.IsNotNull(provider);

        provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);

        return
        (
            provider.CreateGenerator(),
            !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun,
            provider.GenerateMultipleFiles && !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName),
            provider.CreateAdditionalParameters(),
            templateFileManagerFactory.Create(basePath)
        );
    }

    internal static void ProcessResult(ICodeGenerationProvider provider, bool shouldSave, bool shouldUseLastGeneratedFiles, ITemplateFileManager templateFileManager)
    {
        templateFileManager.Process(true, shouldSave);

        if (shouldSave)
        {
            if (shouldUseLastGeneratedFiles)
            {
                var prefixedLastGeneratedFilesFileName = Path.Combine(provider.Path, provider.LastGeneratedFilesFileName);
                templateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFileName, provider.RecurseOnDeleteGeneratedFiles);
                templateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
            }

            templateFileManager.SaveAll();
        }
    }
}

public class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(string basePath = "")
        : this(new TemplateRenderer(), new TemplateFileManagerFactory(), basePath)
    {
    }

    internal CodeGenerationEngine(ITemplateRenderer templateRenderer, ITemplateFileManagerFactory templateFileManagerFactory, string basePath = "")
    {
        Guard.IsNotNull(templateRenderer);
        Guard.IsNotNull(basePath);

        _templateFileManagerFactory = templateFileManagerFactory;
        _templateRenderer = templateRenderer;
        _basePath = basePath;
    }

    private readonly ITemplateFileManagerFactory _templateFileManagerFactory;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly string _basePath;

    public void Generate(ICodeGenerationProvider provider, ICodeGenerationSettings settings)
    {
        var result = CodeGenerationEngineBase.Initialize(provider, settings, _templateFileManagerFactory, _basePath);
        if (provider.GenerateMultipleFiles)
        {
            _templateRenderer.Render(template: result.generator,
                                     generationEnvironment: result.templateFileManager.MultipleContentBuilder,
                                     defaultFileName: provider.DefaultFileName,
                                     additionalParameters: result.additionalParameters);
        }
        else
        {
            _templateRenderer.Render(template: result.generator,
                                     generationEnvironment: result.templateFileManager.StartNewFile(Path.Combine(provider.Path, provider.DefaultFileName)),
                                     defaultFileName: string.Empty,
                                     additionalParameters: result.additionalParameters);
        }

        CodeGenerationEngineBase.ProcessResult(provider, result.shouldSave, result.shouldUseLastGeneratedFiles, result.templateFileManager);
    }
}

public class CodeGenerationEngine<T> : ICodeGenerationEngine<T>
{
    public CodeGenerationEngine(string basePath = "")
        : this(new TemplateRenderer<T>(), new TemplateFileManagerFactory(), basePath)
    {
    }

    internal CodeGenerationEngine(ITemplateRenderer<T> templateRenderer, ITemplateFileManagerFactory templateFileManagerFactory, string basePath = "")
    {
        Guard.IsNotNull(templateRenderer);
        Guard.IsNotNull(basePath);

        _templateFileManagerFactory = templateFileManagerFactory;
        _templateRenderer = templateRenderer;
        _basePath = basePath;
    }

    private readonly ITemplateFileManagerFactory _templateFileManagerFactory;
    private readonly ITemplateRenderer<T> _templateRenderer;
    private readonly string _basePath;

    public void Generate(ICodeGenerationProvider<T> provider, ICodeGenerationSettings settings)
    {
        var result = CodeGenerationEngineBase.Initialize(provider, settings, _templateFileManagerFactory, _basePath);
        if (provider.GenerateMultipleFiles)
        {
            _templateRenderer.Render(template: result.generator,
                                     generationEnvironment: result.templateFileManager.MultipleContentBuilder,
                                     model: provider.CreateModel(),
                                     defaultFileName: provider.DefaultFileName,
                                     additionalParameters: result.additionalParameters);
        }
        else
        {
            _templateRenderer.Render(template: result.generator,
                                     generationEnvironment: result.templateFileManager.StartNewFile(Path.Combine(provider.Path, provider.DefaultFileName)),
                                     model: provider.CreateModel(),
                                     defaultFileName: string.Empty,
                                     additionalParameters: result.additionalParameters);
        }

        CodeGenerationEngineBase.ProcessResult(provider, result.shouldSave, result.shouldUseLastGeneratedFiles, result.templateFileManager);
    }
}
