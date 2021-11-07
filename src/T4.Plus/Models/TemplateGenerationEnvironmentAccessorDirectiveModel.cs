using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Sets the accessor (visibility) of the GenerationEnvironment property of the template")]
    public class TemplateGenerationEnvironmentAccessorDirectiveModel
    {
        [Description("Accessor (visibility) of the GenerationEnvironment property of the template (e.g. protected, internal, public)")]
        [Required]
        public string Value { get; set; }
    }
}
