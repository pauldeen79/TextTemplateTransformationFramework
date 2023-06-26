namespace TextTemplateTransformationFramework.Core.Models;

internal class Content : IContent
{
    public Content() : this(new IndentedStringBuilder())
    {
    }

    public Content(IIndentedStringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Builder = builder;
    }

    public string FileName { get; set; } = "";

    public bool SkipWhenFileExists { get; set; }

    public IIndentedStringBuilder Builder { get; }
}
