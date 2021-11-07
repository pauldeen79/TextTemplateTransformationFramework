using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.Common.Models
{
    [Description("Defines a property on the template")]
    public class PropertyDirectiveModel
    {
        [Required]
        [Description("Name of the property")]
        public string Name { get; set; }

        [DataMember(Name = "type")]
        [Description("Assembly-qualified name of the property type")]
        [Required]
        public string TypeName { get; set; }

        [DataMember(Name = "genericTypeParameter")]
        [Description("When the type is a generic type, then this is the assembly-qualified name of the generic parameter type")]
        public string GenericParameterTypeName { get; set; }

        [Description("Indicator whether the property should be compatible for .Net core (true by default). Set to false for support for .Net remoting.")]
        [DefaultValue(true)]
        public bool NetCoreCompatible { get; set; }
    }
}
