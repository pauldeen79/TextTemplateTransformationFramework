using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.Common.Models
{
    [Description("Adds an assembly reference")]
    public class AssemblyDirectiveModel
    {
        [Required]
        [Description("Name of the assembly reference to add")]
        public string Name { get; set; }
    }
}
