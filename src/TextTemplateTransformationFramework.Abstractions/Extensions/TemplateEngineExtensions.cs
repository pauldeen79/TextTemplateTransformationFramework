namespace TemplateFramework.Abstractions.Extensions;

public static class TemplateEngineExtensions
{
    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, object model)
        => instance.Render(template, generationEnvironment, model, string.Empty, null, null);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, object model, string defaultFilename)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null, null);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, object? model, object additionalParameters)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters, null);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, object model, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, string.Empty, null, context);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, object model, string defaultFilename, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null, context);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, object model, object additionalParameters, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters, context);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment)
        => instance.Render(template, generationEnvironment, default(object?), string.Empty, null, null);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, string defaultFilename)
        => instance.Render(template, generationEnvironment, default(object?), defaultFilename, null, null);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, ITemplateContext context)
        => instance.Render(template, generationEnvironment, default(object?), string.Empty, null, context);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, string defaultFilename, ITemplateContext context)
        => instance.Render(template, generationEnvironment, default(object?), defaultFilename, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, object model)
        => instance.Render(template, generationEnvironment, model, string.Empty, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, object model, string defaultFilename)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, object? model, object additionalParameters)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, object model, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, string.Empty, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, object model, string defaultFilename, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, object model, object additionalParameters, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment)
        => instance.Render(template, generationEnvironment, default(object?), string.Empty, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, string defaultFilename)
        => instance.Render(template, generationEnvironment, default(object?), defaultFilename, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, ITemplateContext context)
        => instance.Render(template, generationEnvironment, default(object?), string.Empty, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, string defaultFilename, ITemplateContext context)
        => instance.Render(template, generationEnvironment, default(object?), defaultFilename, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, object model)
        => instance.Render(template, generationEnvironment, model, string.Empty, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, object model, string defaultFilename)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, object? model, object additionalParameters)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, object model, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, string.Empty, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, object model, string defaultFilename, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, object model, object additionalParameters, ITemplateContext context)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment)
        => instance.Render(template, generationEnvironment, default(object?), string.Empty, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, string defaultFilename)
        => instance.Render(template, generationEnvironment, default(object?), defaultFilename, null, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, ITemplateContext context)
        => instance.Render(template, generationEnvironment, default(object?), string.Empty, null, context);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, string defaultFilename, ITemplateContext context)
        => instance.Render(template, generationEnvironment, default(object?), defaultFilename, null, context);
}
