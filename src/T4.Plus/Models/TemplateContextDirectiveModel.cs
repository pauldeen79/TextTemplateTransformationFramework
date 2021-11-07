using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Allows to add Template Context to the template base class")]
    public class TemplateContextDirectiveModel
    {
        [Description("Indicator whether Template Context support needs to be added to the template base class")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        [Description("When set to true, only includes initialization for child template classs")]
        public bool Override { get; set; }
    }
}
