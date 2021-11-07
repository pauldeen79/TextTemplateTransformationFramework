using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Enables or disables generation of ExcludeFromCodeCoverage attributes on generated classes")]
    public class AddExcludeFromCodeCoverageAttributesDirectiveModel
    {
        [Description("When set to true, generates ExcludeFromCodeCoverge attributes on generated classes")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
