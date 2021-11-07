using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public abstract class MessageDirectiveModel
    {
        [Required]
        public string Message { get; set; }
    }
}
