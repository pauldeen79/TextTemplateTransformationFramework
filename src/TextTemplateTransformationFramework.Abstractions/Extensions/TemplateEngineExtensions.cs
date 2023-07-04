﻿namespace TextTemplateTransformationFramework.Abstractions.Extensions;

public static class TemplateEngineExtensions
{
    public static void Render<T>(this ITemplateEngine<T> instance, object template, StringBuilder generationEnvironment, T model)
        => instance.Render(template, generationEnvironment, model, string.Empty, null);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, StringBuilder generationEnvironment, T model, string defaultFilename)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, StringBuilder generationEnvironment, T model, object additionalParameters)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, IMultipleContentBuilder generationEnvironment, T model)
        => instance.Render(template, generationEnvironment, model, string.Empty, null);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, IMultipleContentBuilder generationEnvironment, T model, string defaultFilename)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, IMultipleContentBuilder generationEnvironment, T model, object additionalParameters)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, IMultipleContentBuilderContainer generationEnvironment, T model)
        => instance.Render(template, generationEnvironment, model, string.Empty, null);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, IMultipleContentBuilderContainer generationEnvironment, T model, string defaultFilename)
        => instance.Render(template, generationEnvironment, model, defaultFilename, null);

    public static void Render<T>(this ITemplateEngine<T> instance, object template, IMultipleContentBuilderContainer generationEnvironment, T model, object additionalParameters)
        => instance.Render(template, generationEnvironment, model, string.Empty, additionalParameters);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment)
        => instance.Render(template, generationEnvironment, string.Empty, null);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, string defaultFilename)
        => instance.Render(template, generationEnvironment, defaultFilename, null);

    public static void Render(this ITemplateEngine instance, object template, StringBuilder generationEnvironment, object additionalParameters)
        => instance.Render(template, generationEnvironment, string.Empty, additionalParameters);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment)
        => instance.Render(template, generationEnvironment, string.Empty, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, string defaultFilename)
        => instance.Render(template, generationEnvironment, defaultFilename, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilder generationEnvironment, object additionalParameters)
        => instance.Render(template, generationEnvironment, string.Empty, additionalParameters);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment)
        => instance.Render(template, generationEnvironment, string.Empty, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, string defaultFilename)
        => instance.Render(template, generationEnvironment, defaultFilename, null);

    public static void Render(this ITemplateEngine instance, object template, IMultipleContentBuilderContainer generationEnvironment, object additionalParameters)
        => instance.Render(template, generationEnvironment, string.Empty, additionalParameters);
}
