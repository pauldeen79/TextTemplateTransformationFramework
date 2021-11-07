using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Enables or disables copying propeties from parent template to view model")]
    public class CopyPropertiesToViewModelDirectiveModel
    {
        [Description("When set to true, includes code in the template footer that allows to use child templates")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
