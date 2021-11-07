using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Sets the template name (only used for child templates)")]
    public class TemplateNameDirectiveModel
    {
        [Description("Name of the template")]
        [Required]
        public string Value { get; set; }
    }
}
