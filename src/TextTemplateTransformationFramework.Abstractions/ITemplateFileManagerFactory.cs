namespace TextTemplateTransformationFramework.Abstractions;

public interface ITemplateFileManagerFactory
{
    ITemplateFileManager Create(string basePath);
}
