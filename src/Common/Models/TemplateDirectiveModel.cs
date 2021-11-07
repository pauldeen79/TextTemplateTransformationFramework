using System.ComponentModel;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.Common.Models
{
    public class TemplateDirectiveModel
    {
        [Description("Script language used in the template (Currently, only C# is supported)")]
        public string Language { get; set; }

        [Description("Template class name, including namespace")]
        [DefaultValue("GeneratedNamespace.GeneratedTemplate")]
        public string ClassName { get; set; }

        [Description("TypeName of the base class, or empty to use a dynamically generated baseclass")]
        [DataMember(Name = "inherits")]
        public string BaseClassName { get; set; }

        [Description("Optional culture code to use in rendering")]
        public string Culture { get; set; }
    }
}
