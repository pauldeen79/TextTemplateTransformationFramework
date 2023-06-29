namespace TextTemplateTransformationFramework.Core.Models;

internal class Content : IContent
{
    public Content(StringBuilder builder, bool skipWhenFileExists, string fileName)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNullOrWhiteSpace(fileName);

        Builder = builder;
        SkipWhenFileExists = skipWhenFileExists;
        FileName = fileName;
    }

    public string FileName { get; }

    public bool SkipWhenFileExists { get; }

    public StringBuilder Builder { get; }
}
