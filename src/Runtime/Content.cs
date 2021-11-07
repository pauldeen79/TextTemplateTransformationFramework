using System.Text;

namespace TextTemplateTransformationFramework.Runtime
{
    public class Content
    {
        public Content()
        {
            Builder = new StringBuilder();
        }

        public Content(StringBuilder builder)
        {
            Builder = builder;
        }

        public string FileName { get; set; }

        public bool SkipWhenFileExists { get; set; }

        public StringBuilder Builder { get; }
    }
}