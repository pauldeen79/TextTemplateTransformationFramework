using System.Text;

namespace TextTemplateTransformationFramework.Runtime.CodeGeneration
{
    public class GenerateCode
    {
        private const string Parent = "/";

        private GenerateCode(string basePath, IMultipleContentBuilder multipleContentBuilder = null)
        {
            GenerationEnvironment = new StringBuilder();
            TemplateFileManager = new TemplateFileManager(b => GenerationEnvironment = b,
                                                          () => GenerationEnvironment,
                                                          basePath,
                                                          multipleContentBuilder);
        }

        public TemplateFileManager TemplateFileManager { get; }
        public StringBuilder GenerationEnvironment { get; private set; }

        public static GenerateCode For<T>(CodeGenerationSettings settings)
            where T : ICodeGenerationProvider, new()
            => For<T>(settings, null);

        public static GenerateCode For<T>(CodeGenerationSettings settings, IMultipleContentBuilder multipleContentBuilder)
            where T : ICodeGenerationProvider, new()
        {
            var provider = new T();
            provider.Initialize(settings.GenerateMultipleFiles, settings.SkipWhenFileExists, settings.BasePath);
            var result = new GenerateCode(provider.BasePath, multipleContentBuilder);
            var generator = provider.CreateGenerator();
            var shouldSave = !string.IsNullOrEmpty(provider.BasePath) && !settings.DryRun;
            var shouldUseLastGeneratedFiles = !string.IsNullOrEmpty(provider.LastGeneratedFilesFileName);
            var additionalParameters = provider.CreateAdditionalParameters();

            TemplateRenderHelper.RenderTemplateWithModel(template: generator,
                                                         generationEnvironment: provider.GenerateMultipleFiles
                                                            ? result.TemplateFileManager
                                                            : result.TemplateFileManager.StartNewFile(provider.Path + Parent + provider.DefaultFileName),
                                                         model: provider.CreateModel(),
                                                         defaultFileName: provider.GenerateMultipleFiles
                                                            ? provider.DefaultFileName
                                                            : null,
                                                         additionalActionDelegate: provider.AdditionalActionDelegate,
                                                         additionalParameters: additionalParameters);

            result.TemplateFileManager.Process(true, shouldSave);

            if (shouldSave)
            {
                if (shouldUseLastGeneratedFiles)
                {
                    var prefixedLastGeneratedFilesFileName = provider.Path + Parent + (settings.GenerateMultipleFiles ? provider.LastGeneratedFilesFileName : provider.DefaultFileName);
                    result.TemplateFileManager.DeleteLastGeneratedFiles(prefixedLastGeneratedFilesFileName, provider.RecurseOnDeleteGeneratedFiles);
                    result.TemplateFileManager.SaveLastGeneratedFiles(prefixedLastGeneratedFilesFileName);
                }
                result.TemplateFileManager.SaveAll();
            }
            return result;
        }
    }
}
