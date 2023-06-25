namespace TextTemplateTransformationFramework.Core;

public class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(string basePath = "")
        : this(new TemplateRenderer(), () => new TemplateFileManager(new StringBuilder(), basePath))
    {
    }

    internal CodeGenerationEngine(ITemplateRenderer templateRenderer, Func<ITemplateFileManager> templateFileManagerDelegate)
    {
        Guard.IsNotNull(templateRenderer);

        _templateFileManagerDelegate = templateFileManagerDelegate;
        _templateRenderer = templateRenderer;
    }

    private readonly Func<ITemplateFileManager> _templateFileManagerDelegate;
    private readonly ITemplateRenderer _templateRenderer;

    public void Generate(ICodeGenerationProvider provider, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(settings);
        Guard.IsNotNull(provider);

        provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);

        var generator = provider.CreateGenerator();
        var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun;
        var shouldUseLastGeneratedFiles = provider.GenerateMultipleFiles && !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName);
        var additionalParameters = provider.CreateAdditionalParameters();

        var templateFileManager = _templateFileManagerDelegate.Invoke();
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
