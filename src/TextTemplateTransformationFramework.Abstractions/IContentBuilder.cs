namespace TextTemplateTransformationFramework.Abstractions
{
    public interface IContentBuilder
    {
        string? FileName { get; set; }
        bool SkipWhenFileExists { get; set; }
        StringBuilder Builder { get; }
        IContent Build();
    }
}
