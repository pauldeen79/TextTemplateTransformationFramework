using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Skips initialization code in the generated template")]
    public class SkipInitializationCodeDirectiveModel
    {
        [Description("Indicator whether to skip initialization (default is true)")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
