using System.ComponentModel;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Defines a property on the template")]
    public class PropertyDirectiveModel : TextTemplateTransformationFramework.Common.Models.PropertyDirectiveModel
    {
        [Description("Default value of the property")]
        public string DefaultValue { get; set; }

        [Description("Indicator whether the default value is a string literal. When set to false, it is an expression")]
        public bool DefaultValueIsLiteral { get; set;  }

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

        [Description("Optional editor typename for EditorAttribute (used in conjunction with EditorAttributeEditorBaseTypeName)")]
        public string EditorAttributeEditorTypeName { get; set; }

        [Description("Optional editor base type for EditorAttribute (used in conjunction with EditorAttributeEditorTypeName)")]
        public string EditorAttributeEditorBaseType { get; set; }

        [Description("Optional typename for TypeConverterAttribute")]
        public string TypeConverterTypeName { get; set; }

        [Description("Optional category")]
        public string Category { get; set; }

        [Description("Indicator whether the initialization should be skipped entirely")]
        public bool OmitInitialization{ get; set; }
    }
}
