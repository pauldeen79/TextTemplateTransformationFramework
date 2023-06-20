namespace Utilities
{
    public static class LazyEnumParser
    {
        public static LazyEnumParseResult<T> Parse<T>(string value, bool ignoreCase)
            where T : struct
            => new LazyEnumParseResult<T>(value, ignoreCase);
    }
}
