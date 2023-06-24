using System.Text;
using TextTemplateTransformationFramework.Abstractions;

namespace TextTemplateTransformationFramework.Core
{
    public class CodeGenerationEngine : ICodeGenerationEngine
    {
        private const string Parent = "/";

        //TODO: Review if we need a default c'tor, or refactor to use DI
        public CodeGenerationEngine(string basePath = "")
            : this(new TemplateRenderer(), new TemplateFileManager(new StringBuilder(), basePath))
        {
        }

        public CodeGenerationEngine(ITemplateRenderer templateRenderer, ITemplateFileManager templateFileManager)
        {
            if (templateFileManager is null)
            {
                throw new ArgumentNullException(nameof(templateFileManager));
            }

            if (templateRenderer is null)
            {
                throw new ArgumentNullException(nameof(templateRenderer));
            }

            _templateFileManager = templateFileManager;
            _templateRenderer = templateRenderer;
        }

        private readonly ITemplateFileManager _templateFileManager;
        private readonly ITemplateRenderer _templateRenderer;

        public void Generate(ICodeGenerationProvider provider, ICodeGenerationSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);

            var generator = provider.CreateGenerator();
            var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun;
            var shouldUseLastGeneratedFiles = !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName);
            var additionalParameters = provider.CreateAdditionalParameters();

            _templateRenderer.Render(template: generator,
                                     generationEnvironment: provider.GenerateMultipleFiles
                                          ? _templateFileManager
                                          : _templateFileManager.StartNewFile($"{provider.Path}{Parent}{provider.DefaultFileName}"),
                                     model: provider.CreateModel(),
                                     defaultFileName: provider.GenerateMultipleFiles
                                          ? provider.DefaultFileName
                                          : string.Empty,
                                     additionalParameters: additionalParameters);

            _templateFileManager.Process(true, shouldSave);

            if (shouldSave)
            {
                if (shouldUseLastGeneratedFiles)
                {
                    var filename = provider.GenerateMultipleFiles ? provider.LastGeneratedFilesFileName : provider.DefaultFileName;
                    var prefixedLastGeneratedFilesFileName = $"{provider.Path}{Parent}{filename}";
                    _templateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFileName, provider.RecurseOnDeleteGeneratedFiles);
                    _templateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
                }

                _templateFileManager.SaveAll();
            }
        }
    }
}
