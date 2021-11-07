using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Defines content to render from a directive argument")]
    public class RenderArgumentDirectiveModel
    {
        [Required]
        [Description("Content to render")]
        public string Argument { get; set; }
    }
}
