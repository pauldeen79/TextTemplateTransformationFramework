using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.Common.Models
{
    [Description("Sets the extension of the output file")]
    public class OutputDirectiveModel
    {
        [Required]
        [Description("Extension of the output file")]
        public string Extension { get; set; }
    }
}
