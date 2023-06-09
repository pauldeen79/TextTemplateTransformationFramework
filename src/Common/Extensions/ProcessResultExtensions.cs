using System;
using System.Linq;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class ProcessResultExtensions
    {
        public static bool IsValid(this ProcessResult instance)
            => Array.TrueForAll(instance.CompilerErrors, e => e.IsWarning);
    }
}
