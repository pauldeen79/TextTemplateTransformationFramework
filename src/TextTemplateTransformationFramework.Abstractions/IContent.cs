namespace TextTemplateTransformationFramework.Abstractions;

public interface IContent
{
    string FileName { get; }
    bool SkipWhenFileExists { get; }
    StringBuilder Builder { get; }
}
