using System;
using System.Collections.Generic;
using System.Linq;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class EnumerableOfStringExtensions
    {
        public static IEnumerable<T> GetComponents<T>(this IEnumerable<string> instance) =>
            instance == null
                ? Array.Empty<T>()
                : instance
                    .Select(s => Activator.CreateInstance(Type.GetType(s)))
                    .Cast<T>();
    }
}
