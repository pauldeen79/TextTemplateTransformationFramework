using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Allows to generated overrides for Render and Initialize methods, instead of virtual")]
    public class OverrideTemplateDirectiveModel
    {
        [Description("When set to true, generates overrides for Render and Initialize methods, instead of virtual")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
