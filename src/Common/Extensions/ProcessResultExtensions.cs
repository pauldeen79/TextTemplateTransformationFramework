using System.Linq;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class ProcessResultExtensions
    {
        public static bool IsValid(this ProcessResult instance) =>
            instance.CompilerErrors.All(e => e.IsWarning);
    }
}
