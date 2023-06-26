namespace TextTemplateTransformationFramework.Core;

internal class TemplateFileManagerFactory : ITemplateFileManagerFactory
{
    public ITemplateFileManager Create(string basePath) => new TemplateFileManager(new IndentedStringBuilder(), basePath);
}
