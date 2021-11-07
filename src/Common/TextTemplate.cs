using System;

namespace TextTemplateTransformationFramework.Common
{
    public class TextTemplate
    {
        public string Template { get; }
        public string FileName { get; }

        public TextTemplate(string template, string filename = "unknown.tt")
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            FileName = filename ?? throw new ArgumentNullException(nameof(filename));
        }
    }
}
