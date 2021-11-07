using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Sets the base path")]
    public class BasePathDirectiveModel
    {
        [Description("Base path for files")]
        [Required]
        public string Value { get; set; }
    }
}
