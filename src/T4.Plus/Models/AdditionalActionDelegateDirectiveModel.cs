using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Allows to inject an additional delegate in the Initialize method of the template class")]
    public class AdditionalActionDelegateDirectiveModel
    {
        [Description("Indicator whether an additional action delegate should be added as argument to the Initialize method of the template class")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }

        [Description("Indicator whether the additional action delegate should be invoked inside this template. Set to true when the call is performed more than once. (some inherited template secnarios)")]
        public bool SkipInitializationCode { get; set; }
    }
}
