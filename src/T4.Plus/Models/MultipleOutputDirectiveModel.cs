using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Use template file manager to allow creation of multiple output")]
    public class MultipleOutputDirectiveModel
    {
        [Description("When set to true, includes a template file manager that allows to create multiple output (default is true)")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
