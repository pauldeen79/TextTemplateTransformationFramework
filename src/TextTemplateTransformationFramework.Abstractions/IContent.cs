namespace TextTemplateTransformationFramework.Abstractions;

public interface IContent
{
    string Filename { get; }
    bool SkipWhenFileExists { get; }
    StringBuilder Builder { get; }
}
