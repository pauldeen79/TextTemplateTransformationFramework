using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.T4.Extensions
{
    public static class SectionContextExtensions
    {
        public static bool IsValidForProcessors<TState>(this SectionContext<TState> context)
            where TState : class
            => !string.IsNullOrEmpty(context.Section);

        public static bool IsText<TState>(this SectionContext<TState> context)
            where TState : class
            => !string.IsNullOrEmpty(context.Section) && context.CurrentMode.IsTextRange();
    }
}
