namespace TextTemplateTransformationFramework.Core;

public class CodeGenerationEngine : CodeGenerationEngine<object?>
{
    public CodeGenerationEngine(string basePath = "") : base(basePath)
    {
    }

    internal CodeGenerationEngine(ITemplateRenderer<object?> templateRenderer, ITemplateFileManagerFactory templateFileManagerFactory, string basePath = "") : base(templateRenderer, templateFileManagerFactory, basePath)
    {
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
        Guard.IsNotNull(settings);
        Guard.IsNotNull(provider);

        provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);

        var generator = provider.CreateGenerator();
        var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun;
        var shouldUseLastGeneratedFiles = provider.GenerateMultipleFiles && !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName);
        var additionalParameters = provider.CreateAdditionalParameters();

        var templateFileManager = _templateFileManagerFactory.Create(_basePath);
        if (provider.GenerateMultipleFiles)
        {
            _templateRenderer.Render(template: generator,
                                     generationEnvironment: templateFileManager.MultipleContentBuilder,
                                     model: provider.CreateModel(),
                                     defaultFileName: provider.DefaultFileName,
                                     additionalParameters: additionalParameters);
        }
        else
        {
            _templateRenderer.Render(template: generator,
                                     generationEnvironment: templateFileManager.StartNewFile(Path.Combine(provider.Path, provider.DefaultFileName)),
                                     model: provider.CreateModel(),
                                     defaultFileName: string.Empty,
                                     additionalParameters: additionalParameters);
        }

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
