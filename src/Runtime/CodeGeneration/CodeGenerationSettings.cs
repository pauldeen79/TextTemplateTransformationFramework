namespace TextTemplateTransformationFramework.Runtime.CodeGeneration
{
    public class CodeGenerationSettings
    {
        public CodeGenerationSettings(string basePath, bool generateMultipleFiles, bool skipWhenFileExists, bool dryRun)
        {
            BasePath = basePath;
            GenerateMultipleFiles = generateMultipleFiles;
            SkipWhenFileExists = skipWhenFileExists;
            DryRun = dryRun;
        }

        public string BasePath { get; }
        public bool GenerateMultipleFiles { get; }
        public bool SkipWhenFileExists { get; }
        public bool DryRun { get; }
    }
}
