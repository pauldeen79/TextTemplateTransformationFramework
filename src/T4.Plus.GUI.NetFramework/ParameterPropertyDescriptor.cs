using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4.Plus.GUI
{
    /// <summary>
    /// Helper class for creating a property descriptor from a TemplateParameter.
    /// </summary>
    public class ParameterPropertyDescriptor : PropertyDescriptor
    {
        private readonly TemplateParameter TemplateParameter;
        private readonly Type Type;

        public ParameterPropertyDescriptor(string name, Attribute[] attrs, TemplateParameter templateParameter, Type componentType)
            : base(name, attrs)
        {
            TemplateParameter = templateParameter;
            Type = componentType;
        }

        public override bool CanResetValue(object component) => true;

        public override Type ComponentType => Type;

        public override object GetValue(object component) => TemplateParameter.Value;

        public override bool IsReadOnly => false;

        public override Type PropertyType => TemplateParameter.Type;

        public override void ResetValue(object component) => TemplateParameter.Value = null;

        public override void SetValue(object component, object value) => TemplateParameter.Value = value;

        public override bool ShouldSerializeValue(object component) => false;
    }
}
