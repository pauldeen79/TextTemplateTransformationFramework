namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class IntExtensions
    {
        public static bool IsInRange(this int value, int minimum, int maximum) =>
            value >= minimum && value <= maximum;

        public static bool IsUnknownRange(this int value) =>
            value.IsInRange(Mode.UnknownRangeStart, Mode.UnknownRangeEnd);

        public static bool IsTextRange(this int value) =>
            value.IsInRange(Mode.TextRangeStart, Mode.TextRangeEnd);

        public static bool IsExpressionRange(this int value) =>
            value.IsInRange(Mode.ExpressionRangeStart, Mode.ExpressionRangeEnd);

        public static bool IsCodeRange(this int value) =>
            value.IsInRange(Mode.CodeRangeStart, Mode.CodeRangeEnd);
    }
}
