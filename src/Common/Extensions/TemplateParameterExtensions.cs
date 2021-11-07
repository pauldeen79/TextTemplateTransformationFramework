using System;
using System.Reflection;

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
                : ConvertProperty(parameter, property);
        }

        private static object ConvertProperty(TemplateParameter parameter, PropertyInfo property)
            => parameter.Value is int && property.PropertyType.IsEnum
                ? Enum.ToObject(property.PropertyType, parameter.Value)
                : Convert.ChangeType(parameter.Value, property.PropertyType);
    }
}
