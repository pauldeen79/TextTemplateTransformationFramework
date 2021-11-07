using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class AddTemplateToPlaceholderDirectiveModel
    {
        [Required]
        [Description("Name of the placeholder to add a child template to")]
        public string Name { get; set; }

        [DefaultValue(true)]
        [Description("Indicator whether the name is a string literal. When set to false, it is an expression")]
        public bool NameIsLiteral { get; set; }

        [Required]
        [Description("Name of the child template to add to the placeholder")]
        public string ChildTemplateName { get; set; }

        [DefaultValue(true)]
        [Description("Indicator whether the child template name is a string literal. When set to false, it is an expression")]
        public bool ChildTemplateNameIsLiteral { get; set; }
    }
}
