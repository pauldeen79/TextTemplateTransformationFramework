namespace TemplateFramework.Abstractions.Extensions;

public static class TemplateContextExtensions
{
    public static T? GetModelFromContextByType<T>(this ITemplateContext instance)
        => instance.GetModelFromContextByType<T>(null);

    public static T? GetViewModelFromContextByType<T>(this ITemplateContext instance)
        => instance.GetViewModelFromContextByType<T>(null);

    public static T? GetContextByTemplateType<T>(this ITemplateContext instance)
        => instance.GetContextByTemplateType<T>(null);

    public static ITemplateContext CreateChildContext(this ITemplateContext instance, object template)
        => instance.CreateChildContext(template, null, null, null, null);

    public static ITemplateContext CreateChildContext(this ITemplateContext instance, object template, object? model)
    => instance.CreateChildContext(template, model, null, null, null);

    public static ITemplateContext CreateChildContext(this ITemplateContext instance, object template, object? model, object? viewModel)
        => instance.CreateChildContext(template, model, viewModel, null, null);

    public static ITemplateContext CreateChildContext(this ITemplateContext instance, object template, object? model, int? iterationNumber, int? iterationCount)
        => instance.CreateChildContext(template, model, null, iterationNumber, iterationCount);
}
