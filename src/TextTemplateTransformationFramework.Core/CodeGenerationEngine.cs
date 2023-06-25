namespace TextTemplateTransformationFramework.Core;

public class CodeGenerationEngine : ICodeGenerationEngine
{
    public CodeGenerationEngine(string basePath = "")
        : this(new TemplateRenderer(), new TemplateFileManager(new StringBuilder(), basePath))
    {
    }

    internal CodeGenerationEngine(ITemplateRenderer templateRenderer, ITemplateFileManager templateFileManager)
    {
        Guard.IsNotNull(templateRenderer);
        Guard.IsNotNull(templateFileManager);

        _templateFileManager = templateFileManager;
        _templateRenderer = templateRenderer;
    }

    private readonly ITemplateFileManager _templateFileManager;
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

        if (provider.GenerateMultipleFiles)
        {
            _templateRenderer.Render(template: generator,
                                     generationEnvironment: _templateFileManager.MultipleContentBuilder,
                                     model: provider.CreateModel(),
                                     defaultFileName: provider.DefaultFileName,
                                     additionalParameters: additionalParameters);
        }
        else
        {
            _templateRenderer.Render(template: generator,
                                     generationEnvironment: _templateFileManager.StartNewFile(Path.Combine(provider.Path, provider.DefaultFileName)),
                                     model: provider.CreateModel(),
                                     defaultFileName: string.Empty,
                                     additionalParameters: additionalParameters);
        }

        _templateFileManager.Process(true, shouldSave);

        if (shouldSave)
        {
            if (shouldUseLastGeneratedFiles)
            {
                var prefixedLastGeneratedFilesFileName = Path.Combine(provider.Path, provider.LastGeneratedFilesFileName);
                _templateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFileName, provider.RecurseOnDeleteGeneratedFiles);
                _templateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
            }

            _templateFileManager.SaveAll();
        }
    }
}
