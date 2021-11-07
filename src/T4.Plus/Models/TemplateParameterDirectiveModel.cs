using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Defines a template parameter")]
    public class TemplateParameterDirectiveModel
    {
        [Required]
        [Description("Name of the template parameter")]
        public string Name { get; set; }

        [Required]
        [Description("Value of the template parameter")]
        public string Value { get; set; }
    }
}
