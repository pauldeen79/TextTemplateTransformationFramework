using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Sets the temporary path")]
    public class TempPathDirectiveModel
    {
        [Description("Temporary path for files")]
        [Required]
        public string Value { get; set; }
    }
}
