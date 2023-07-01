namespace TextTemplateTransformationFramework.Abstractions.Extensions;

public static class MultipleContentBuilderExtensions
{
    public static IContentBuilder AddContent(this IMultipleContentBuilder instance)
        => instance.AddContent(string.Empty, false, null);

    public static IContentBuilder AddContent(this IMultipleContentBuilder instance, string fileName)
        => instance.AddContent(fileName, false, null);

    public static IContentBuilder AddContent(this IMultipleContentBuilder instance, string fileName, bool skipWhenFileExists)
        => instance.AddContent(fileName, skipWhenFileExists, null);
}
