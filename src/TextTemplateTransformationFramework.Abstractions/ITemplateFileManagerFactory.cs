namespace TemplateFramework.Abstractions;

public interface ITemplateFileManagerFactory
{
    ITemplateFileManager Create(string basePath);
}
