using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Enables creation for properties on the template base class for each template parameter")]
    public class ParameterPropertiesDirectiveModel
    {
        [Description("Indicator whether properties need to be added to the template base class for each parameter, true by default")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
