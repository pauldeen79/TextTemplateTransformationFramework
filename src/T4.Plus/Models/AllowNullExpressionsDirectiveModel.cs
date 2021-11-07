using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Ignores null references in expressions")]
    public class AllowNullExpressionsDirectiveModel
    {
        [Description("Indicator whether to allow null value expressions (default is true)")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
