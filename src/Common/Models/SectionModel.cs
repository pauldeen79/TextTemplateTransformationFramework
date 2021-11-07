using System.ComponentModel;

namespace TextTemplateTransformationFramework.Common.Models
{
    [Description("Code used in a section")]
    public class SectionModel
    {
        [Description("Code to use in a section")]
        public string Code { get; set; }
    }
}
