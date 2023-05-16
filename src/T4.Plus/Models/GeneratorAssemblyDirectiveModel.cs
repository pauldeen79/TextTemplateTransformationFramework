using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Runs code generation providers from assembly")]
    public class GeneratorAssemblyDirectiveModel
    {
        [Description("Assembly name")]
        [Required]
        public string AssemblyName { get; set; }
    }
}
