namespace TextTemplateTransformationFramework.Abstractions;

public interface IMultipleContentBuilder
{
    string BasePath { get; set; }
    IEnumerable<IContentBuilder> Contents { get; }
    void SaveAll();
    void SaveLastGeneratedFiles(string lastGeneratedFilesPath);
    void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse);
    IContentBuilder AddContent(string fileName = "", bool skipWhenFileExists = false, StringBuilder? builder = null);
}
