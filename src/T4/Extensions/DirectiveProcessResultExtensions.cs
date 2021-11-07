using System.Linq;

namespace TextTemplateTransformationFramework.T4.Extensions
{
    public static class DirectiveProcessResultExtensions
    {
        public static bool ShouldProcess<TState>(this DirectiveProcessResult<TState> directiveProcessResult)
            where TState : class
            => !directiveProcessResult.Cancel;
    }
}
