namespace Utilities
{
    public static class LazyEnumParser
    {
        public static EnumParseResult<T> Parse<T>(string value, bool ignoreCase)
            where T : struct
            => new EnumParseResult<T>(value, ignoreCase);
    }
}
