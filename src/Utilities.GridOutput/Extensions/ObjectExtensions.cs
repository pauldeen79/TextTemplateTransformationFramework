using System;
using System.ComponentModel;

namespace Utilities.GridOutput.Extensions
{
    public static class ObjectExtensions
    {
        public static T ConvertTo<T>(this object value)
            => (T)value.ConvertType(typeof(T));

        public static object ConvertType(this object value, Type type, PropertyDescriptor descriptor = null, Func<PropertyDescriptor, object> customConverterDelegate = null)
        {
            if (customConverterDelegate != null)
            {
                return customConverterDelegate(descriptor);
            }

            if (value == null)
            {
                return null;
            }

            if (type == null)
            {
                return null;
            }

            var s = value.ToString();
            if (s == "[[NULL]]")
            {
                return null;
            }

            if (type == typeof(string))
            {
                return s.Replace("[[TAB]]", "\t");
            }

            if (type.IsEnum)
            {
                if (int.TryParse(s, out int i))
                {
                    return Enum.ToObject(type, i);
                }

                return Enum.Parse(type, s);
            }

            if (type == typeof(DateTime?))
            {
                return Convert.ToDateTime(value);
            }

            return Convert.ChangeType(value, type);
        }
    }
}
