using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Calls the Initialize method on the base class")]
    public class InitializeBaseDirectiveModel
    {
        [Description("When set to true, use additional action delegate")]
        public bool AdditionalActionDelegate { get; set; }
    }
}
