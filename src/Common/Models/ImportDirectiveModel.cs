using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.Common.Models
{
    [Description("Includes a template from another file into the current template")]
    public class ImportDirectiveModel
    {
        [Required]
        [Description("Namespace to import")]
        public string Namespace { get; set; }
    }
}
