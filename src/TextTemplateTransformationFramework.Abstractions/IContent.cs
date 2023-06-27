namespace TextTemplateTransformationFramework.Abstractions;

public interface IContent
{
    string FileName { get; set; }
    bool SkipWhenFileExists { get; set; }
    StringBuilder Builder { get; }
}
