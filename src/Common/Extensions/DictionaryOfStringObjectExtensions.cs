using System;
using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class DictionaryOfStringObjectExtensions
    {
        public static T GetValue<T>(this IDictionary<string, object> instance, string key) =>
            instance?.ContainsKey(key) != true
                ? default
                : (T)Convert.ChangeType(instance[key], typeof(T));

        public static T GetValue<T>(this IDictionary<string, object> instance, string key, T defaultValue) =>
            instance?.ContainsKey(key) != true
                ? defaultValue
                : (T)Convert.ChangeType(instance[key], typeof(T));
    }
}
