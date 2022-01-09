using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Specifies whether to include code in teh template footer that allows to use child templates")]
    public class AddChildTemplateCodeDirectiveModel
    {
        [Description("When set to true, includes code in the template footer that allows to use child templates")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        [Description("When set to true, only includes initialization for child template classs")]
        public bool Override { get; set; }

        [Description("When set to true, and Override is true, then child template fields will be cleared from within the Initialize method")]
        public bool ClearFieldsOnOverride { get; set; }

        [Description("Optional custom base class for child templates")]
        public string BaseClass { get; set; }
    }
}
