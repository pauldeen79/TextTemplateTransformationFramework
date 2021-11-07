using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Defines a property on the template with shadow values from an enum, so it can be consumed without reference to ths assembly that contains the enum")]
    public class ShadowEnumPropertyDirectiveModel
    {
        [Required]
        [Description("Name of the property")]
        public string Name { get; set; }

        [Description("Default value of the property")]
        public string DefaultValue { get; set; }

        [Description("Indicator whether the default value is a string literal. When set to false, it is an expression")]
        public bool DefaultValueIsLiteral { get; set; }

        [Description("Display name of the property")]
        public string DisplayName { get; set; }

        [Description("Description of the property")]
        public string Description { get; set; }

        [DefaultValue(true)]
        [Description("Indicator whether the property is browsable")]
        public bool Browsable { get; set; }

        [Description("Indicator whether the property is read-only")]
        public bool ReadOnly { get; set; }

        [Description("Indicator whether the property is required")]
        public bool Required { get; set; }

        [Description("Indicator whether the default value assignement of the property should be skipped in the initialization phase")]
        public bool OmitValueAssignment { get; set; }

        [Description("Indicator whether the property should have a setter (false by default)")]
        public bool AddPropertySetter { get; set; }

        [Required(ErrorMessage = "The EnumType field is required.")]
        [Description("Typename of the enum to create shadow property for")]
        [DataMember(Name = "enumType")]
        public string EnumTypeName { get; set; }
    }
}
