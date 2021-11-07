using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class AddTemplateLineNumbersDirectiveModel
    {
        [Description("When set to true, adds line numbers directives that point to the template file instead of the generated code")]
        [DefaultValue(true)]
        public bool Enabled { get; set; }
    }
}
