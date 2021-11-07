using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Inherit from the T4PlusGeneratedTemplateBase base class in the TextTemplateTransformationFramework.Runtime assembly")]
    public class UseTemplateRuntimeBaseClassDirectiveModel
    {
        [Description("When set to true, inherit from the T4PlusGeneratedTemplateBase base class in the TextTemplateTransformationFramework.Runtime assembly")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        [Description("When set to true, inherit from the T4PlusComposableGeneratedTemplateBase base class in the TextTemplateTransformationFramework.Runtime assembly")]
        public bool Composable { get; set; }
    }
}
