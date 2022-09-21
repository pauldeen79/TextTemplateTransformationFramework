using System;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TemplateParameterExtensions
    {
        public static object ConvertType(this TemplateParameter parameter, Type type)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var property = type.GetProperty(parameter.Name);

            return property == null || parameter.Value == null || property.PropertyType.IsInstanceOfType(parameter.Value)
                ? parameter.Value
                : parameter.Value.ConvertValue(property.PropertyType);
        }
    }
}
