using System;
using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class DictionaryOfStringObjectExtensions
    {
        public static T GetValue<T>(this IDictionary<string, object> instance, string key)
            => instance == null || !instance.TryGetValue(key, out var value)
                ? default
                : (T)Convert.ChangeType(value, typeof(T));

        public static T GetValue<T>(this IDictionary<string, object> instance, string key, T defaultValue)
            => instance == null || !instance.TryGetValue(key, out var value)
                ? defaultValue
                : (T)Convert.ChangeType(value, typeof(T));
    }
}
