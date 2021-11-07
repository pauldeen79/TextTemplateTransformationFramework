using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Log a message to the diagnostics trace")]
    public class LogDirectiveModel
    {
        [Description("Message to log")]
        [Required]
        public string Message { get; set; }
    }
}
