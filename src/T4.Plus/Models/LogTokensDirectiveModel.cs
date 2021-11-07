using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Logs all tokens to the diagnostic logger")]
    public class LogTokensDirectiveModel
    {
        [Description("When set to true, log all tokens to the diagnostic logger (default is true)")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
