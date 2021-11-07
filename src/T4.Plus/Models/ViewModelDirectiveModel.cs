using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Includes a template from another file into the current template, and uses it as ViewModel")]
    public class ViewModelDirectiveModel
    {
        [Description("Name of the viewmodel that is registered")]
        [Required]
        public string Name { get; set; }

        [Description("Indicator whether the child template name is a literal expression; true by default")]
        [DefaultValue(true)]
        public bool NameIsLiteral { get; set; }

        [Description("Optional typename of the model")]
        [DataMember(Name = "modelType")]
        public string ModelTypeName { get; set; }

        [Description("Optional model expression or literal")]
        public string Model { get; set; }

        [Description("Indicator whether the model is a literal expression; false by default")]
        public bool ModelIsLiteral { get; set; }

        [Description("Indicator whether properties from template should be automatically copied to ViewModel (true by default)")]
        [DataMember(Name = "copyPropertiesFromTemplate")]
        [DefaultValue(true)]
        public bool CopyPropertiesFromTemplate { get; set; }

        [Description("Indicator whether to continue on errors; false by default")]
        public bool SilentlyContinueOnError { get; set; }

        [Description("Custom resolver delegate expression or literal (optional)")]
        public string CustomResolverDelegate { get; set; }

        [Description("Indicator whether the custom resolver delegate is a literal expression; false by default")]
        public bool CustomResolverDelegateIsLiteral { get; set; }

        [Description("Optional model for the resolver delegate; when empty, the model will be used")]
        public string ResolverDelegateModel { get; set; }

        [Description("Indicator whether the resolver delegate model is a literal expression; false by default")]
        public bool ResolverDelegateModelIsLiteral { get; set; }
    }
}
