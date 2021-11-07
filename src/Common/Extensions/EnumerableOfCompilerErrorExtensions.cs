using System.Collections.Generic;
using System.Linq;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class EnumerableOfCompilerErrorExtensions
    {
        public static bool HasErrors(this IEnumerable<CompilerError> instance) =>
            instance.Any(e => !e.IsWarning);
    }
}
