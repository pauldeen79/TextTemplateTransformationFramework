namespace TextTemplateTransformationFramework.Core.Models;

internal class Content : IContent
{
    public Content() : this(new StringBuilder())
    {
    }

    public Content(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Builder = builder;
    }

    public string FileName { get; set; } = "";

    public bool SkipWhenFileExists { get; set; }

    public StringBuilder Builder { get; }
}
