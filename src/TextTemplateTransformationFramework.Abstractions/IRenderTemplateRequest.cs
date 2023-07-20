namespace TemplateFramework.Abstractions;

public interface IRenderTemplateRequest<out TModel>
{
    object Template { get; }
    IGenerationEnvironment GenerationEnvironment { get; }
    string DefaultFilename { get; }
    object? AdditionalParameters { get; }
    ITemplateContext? Context { get; }
    TModel? Model { get; }
}
