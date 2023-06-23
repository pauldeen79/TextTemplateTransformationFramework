using System.Text;
using TextTemplateTransformationFramework.Abstractions;

namespace TextTemplateTransformationFramework.Core
{
    public class Content : IContent
    {
        public Content()
        {
            Builder = new StringBuilder();
        }

        public Content(StringBuilder builder)
        {
            Builder = builder;
        }

        public string FileName { get; set; } = "";

        public bool SkipWhenFileExists { get; set; }

        public StringBuilder Builder { get; }
    }
}
