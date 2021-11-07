using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.Common.Models
{
    [Description("Includes a template from another file into the current template")]
    public class IncludeDirectiveModel
    {
        [Required]
        [Description("File to include")]
        public string File { get; set; }

        [Description("When set to true, the include file will only be included once during one session")]
        public bool Once { get; set; }
    }
}
