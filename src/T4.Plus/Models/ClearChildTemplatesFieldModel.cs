using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Clears child templates field during initialization")]
    public class ClearChildTemplatesFieldModel
    {
        [Description("When set to true, clears the child templates field during initialization")]
        [DefaultValue(true)]
        public bool Enabled { get; }
    }
}
