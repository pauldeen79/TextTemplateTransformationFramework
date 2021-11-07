using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utilities.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetExportedTypes<T>(this Assembly instance)
            => instance
                .GetExportedTypes()
                .Where(t => typeof(T).IsAssignableFrom(t));
    }
}
