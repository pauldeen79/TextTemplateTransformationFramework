using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Specifies generator name and version")]
    public class GeneratorDirectiveModel
    {
        [Required]
        [Description("Name of the generator")]
        public string Name { get; set; }

        [Description("Version of the generator")]
        public string Version { get; set; }
    }
}
