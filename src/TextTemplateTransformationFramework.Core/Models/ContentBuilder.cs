namespace TextTemplateTransformationFramework.Core.Models
{
    public class ContentBuilder : IContentBuilder
    {
        public ContentBuilder() : this(new StringBuilder())
        {
        }

        public ContentBuilder(StringBuilder builder)
        {
            Guard.IsNotNull(builder);
            Builder = builder;
        }

        public string? FileName { get; set; }
        public bool SkipWhenFileExists { get; set; }

        public StringBuilder Builder { get; }

        public IContent Build() => new Content(Builder, SkipWhenFileExists, FileName!);
    }
}
