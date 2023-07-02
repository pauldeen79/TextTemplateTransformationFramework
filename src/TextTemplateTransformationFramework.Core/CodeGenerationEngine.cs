namespace TextTemplateTransformationFramework.Core;

public abstract class CodeGenerationEngineBase
{
    private readonly ITemplateFileManagerFactory _templateFileManagerFactory;
    private readonly string _basePath;

    protected CodeGenerationEngineBase(ITemplateFileManagerFactory templateFileManagerFactory, string basePath = "")
    {
        Guard.IsNotNull(basePath);

        _templateFileManagerFactory = templateFileManagerFactory;
        _basePath = basePath;
    }

    protected (object generator, bool shouldSave, bool shouldUseLastGeneratedFiles, object additionalParameters, ITemplateFileManager templateFileManager) Initialize(ICodeGenerationProvider provider, ICodeGenerationSettings settings)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNull(settings);

        provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);

        return
        (
            provider.CreateGenerator(),
            !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun,
            provider.GenerateMultipleFiles && !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName),
            provider.CreateAdditionalParameters(),
            _templateFileManagerFactory.Create(_basePath)
        );
    }

    protected static void ProcessResult(ICodeGenerationProvider provider, bool shouldSave, bool shouldUseLastGeneratedFiles, ITemplateFileManager templateFileManager)
    {
        Guard.IsNotNull(provider);
        Guard.IsNotNull(templateFileManager);

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

public class CodeGenerationEngine : CodeGenerationEngineBase, ICodeGenerationEngine
{
    public CodeGenerationEngine()
        : this(new TemplateEngine(), new TemplateFileManagerFactory(), string.Empty)
    {
    }

    public CodeGenerationEngine(string basePath)
        : this(new TemplateEngine(), new TemplateFileManagerFactory(), basePath)
    {
    }

    internal CodeGenerationEngine(ITemplateEngine templateEngine,
                                  ITemplateFileManagerFactory templateFileManagerFactory,
                                  string basePath)
        : base(templateFileManagerFactory, basePath)
    {
        Guard.IsNotNull(templateEngine);

        _templateEngine = templateEngine;
    }

    private readonly ITemplateEngine _templateEngine;

    public void Generate(ICodeGenerationProvider provider, ICodeGenerationSettings settings)
    {
        var result = Initialize(provider, settings);
        if (provider.GenerateMultipleFiles)
        {
            _templateEngine.Render(template: result.generator,
                                   generationEnvironment: result.templateFileManager.MultipleContentBuilder,
                                   defaultFileName: provider.DefaultFileName,
                                   additionalParameters: result.additionalParameters);
        }
        else
        {
            _templateEngine.Render(template: result.generator,
                                   generationEnvironment: result.templateFileManager.StartNewFile(Path.Combine(provider.Path, provider.DefaultFileName)),
                                   defaultFileName: string.Empty,
                                   additionalParameters: result.additionalParameters);
        }

        ProcessResult(provider, result.shouldSave, result.shouldUseLastGeneratedFiles, result.templateFileManager);
    }
}

public class CodeGenerationEngine<T> : CodeGenerationEngineBase, ICodeGenerationEngine<T>
{
    public CodeGenerationEngine(string basePath = "")
        : this(new TemplateEngine<T>(), new TemplateFileManagerFactory(), basePath)
    {
    }

    internal CodeGenerationEngine(ITemplateEngine<T> templateEngine,
                                  ITemplateFileManagerFactory templateFileManagerFactory,
                                  string basePath = "")
        : base(templateFileManagerFactory, basePath)
    {
        Guard.IsNotNull(templateEngine);

        _templateEngine = templateEngine;
    }

    private readonly ITemplateEngine<T> _templateEngine;

    public void Generate(ICodeGenerationProvider<T> provider, ICodeGenerationSettings settings)
    {
        var result = Initialize(provider, settings);
        if (provider.GenerateMultipleFiles)
        {
            _templateEngine.Render(template: result.generator,
                                   generationEnvironment: result.templateFileManager.MultipleContentBuilder,
                                   model: provider.CreateModel(),
                                   defaultFileName: provider.DefaultFileName,
                                   additionalParameters: result.additionalParameters);
        }
        else
        {
            _templateEngine.Render(template: result.generator,
                                   generationEnvironment: result.templateFileManager.StartNewFile(Path.Combine(provider.Path, provider.DefaultFileName)),
                                   model: provider.CreateModel(),
                                   defaultFileName: string.Empty,
                                   additionalParameters: result.additionalParameters);
        }

        ProcessResult(provider, result.shouldSave, result.shouldUseLastGeneratedFiles, result.templateFileManager);
    }
}
