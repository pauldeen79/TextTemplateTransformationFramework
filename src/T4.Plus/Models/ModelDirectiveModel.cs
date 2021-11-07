using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Sets properties of the model of the template (only used for child templates)")]
    public class ModelDirectiveModel
    {
        [Description("Typename of the model of the template")]
        [DataMember(Name = "type")]
        [Required]
        public string TypeName { get; set; }

        [DataMember(Name = "genericTypeParameter")]
        [Description("When the type is a generic type, then this is the assembly-qualified name of the generic parameter type")]
        public string GenericParameterTypeName { get; set; }

        [Description("Only use the model for routing")]
        public bool UseForRoutingOnly { get; set; }

        [Description("Use the model for routing, true by default")]
        [DefaultValue(true)]
        public bool UseForRouting { get; set; }

        [Description("Add property setter, false by default")]
        public bool AddPropertySetter { get; set; }
    }
}
