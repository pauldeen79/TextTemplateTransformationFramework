using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Do not use the model for routing")]
    public class SkipModelForRoutingDirectiveModel
    {
        [Description("When set to true, do not add the model type to the routing configuration. In this case, the template can only be called directly.")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
