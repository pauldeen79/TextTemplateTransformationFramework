namespace TemplateFramework.Core;

public class TemplateFileManagerFactory : ITemplateFileManagerFactory
{
    public ITemplateFileManager Create(string basePath) => new TemplateFileManager(new StringBuilder(), basePath);
}
