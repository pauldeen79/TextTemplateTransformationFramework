namespace TextTemplateTransformationFramework.Core.Contracts;

internal interface ITemplateInitializer
{
    void Initialize<T>(object template,
                       string defaultFilename,
                       T? model,
                       object? additionalParameters,
                       ITemplateContext? context);
}
