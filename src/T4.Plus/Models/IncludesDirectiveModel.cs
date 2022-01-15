using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Includes templates from a directory into the current template")]
    public class IncludesDirectiveModel
    {
        [Required]
        [Description("Path of the child templates")]
        public string Path { get; set; }

        [Description("Search pattern")]
        public string SearchPattern { get; set; }

        [Description("Indicator to recursively search the directory; default is false")]
        public bool Recurse { get; set; }

        [Description("When set to true, each include file will only be included once during one session")]
        public bool Once { get; set; }
    }
}
