using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Inherit from the T4PlusGeneratedTemplateBase base class in the TextTemplateTransformationFramework.Runtime assembly")]
    public class UseTemplateRuntimeBaseClassDirectiveModel
    {
        [Description("When set to true, inherit from the T4PlusGeneratedTemplateBase base class in the TextTemplateTransformationFramework.Runtime assembly")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        [Description("When set to true, add the Runtime assembly as reference. Set to false when you get an error about duplicate reference to this assembly")]
        [DefaultValue(true)]
        public bool AddReference { get; set; }
    }
}
