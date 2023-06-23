using System.Text;
using TextTemplateTransformationFramework.Abstractions;

namespace TextTemplateTransformationFramework.Core
{
    public class GenerateCode
    {
        private const string Parent = "/";

        public GenerateCode(string basePath,
                            IMultipleContentBuilder? multipleContentBuilder = null,
                            ITemplateRenderer? templateRenderer = null,
                            ITemplateFileManager? templateFileManager = null)
        {
            GenerationEnvironment = new StringBuilder();
            TemplateFileManager = templateFileManager ?? new TemplateFileManager(
                b => GenerationEnvironment = b,
                () => GenerationEnvironment,
                basePath,
                multipleContentBuilder);
            TemplateRenderer = templateRenderer ?? new TemplateRenderer();
        }

        public ITemplateFileManager TemplateFileManager { get; }
        public ITemplateRenderer TemplateRenderer { get; }

        private StringBuilder GenerationEnvironment { get; set; }

        public static GenerateCode For<T>(CodeGenerationSettings settings)
            where T : ICodeGenerationProvider, new()
            => For<T>(settings, null);

        public static GenerateCode For<T>(CodeGenerationSettings settings, IMultipleContentBuilder? multipleContentBuilder)
            where T : ICodeGenerationProvider, new() => For(settings, new T(), multipleContentBuilder);

        public static GenerateCode For(CodeGenerationSettings settings,
                                       ICodeGenerationProvider provider,
                                       IMultipleContentBuilder? multipleContentBuilder,
                                       ITemplateRenderer? templateRenderer = null)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);
            var result = new GenerateCode(provider.BasePath, multipleContentBuilder, templateRenderer);
            var generator = provider.CreateGenerator();
            var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun;
            var shouldUseLastGeneratedFiles = !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName);
            var additionalParameters = provider.CreateAdditionalParameters();

            result.TemplateRenderer.Render(template: generator,
                                           generationEnvironment: provider.GenerateMultipleFiles
                                                ? result.TemplateFileManager
                                                : result.TemplateFileManager.StartNewFile($"{provider.Path}{Parent}{provider.DefaultFileName}"),
                                           model: provider.CreateModel(),
                                           defaultFileName: provider.GenerateMultipleFiles
                                                ? provider.DefaultFileName
                                                : string.Empty,
                                           additionalParameters: additionalParameters);

            result.TemplateFileManager.Process(true, shouldSave);

            if (shouldSave)
            {
                if (shouldUseLastGeneratedFiles)
                {
                    var filename = provider.GenerateMultipleFiles ? provider.LastGeneratedFilesFileName : provider.DefaultFileName;
                    var prefixedLastGeneratedFilesFileName = $"{provider.Path}{Parent}{filename}";
                    result.TemplateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFileName, provider.RecurseOnDeleteGeneratedFiles);
                    result.TemplateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
                }

                result.TemplateFileManager.SaveAll();
            }

            return result;
        }
    }
}
