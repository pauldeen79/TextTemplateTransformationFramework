namespace TemplateFramework.Core;

public class RenderTemplateRequest<TModel> : IRenderTemplateRequest<TModel>
{
    public RenderTemplateRequest(
        object template,
        StringBuilder generationEnvironment,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(generationEnvironment), defaultFilename, model, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder generationEnvironment,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(generationEnvironment), defaultFilename, model, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer generationEnvironment,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(generationEnvironment), defaultFilename, model, additionalParameters, context)
    {
    }

    private RenderTemplateRequest(
        object template,
        IGenerationEnvironment generationEnvironment,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);
        Guard.IsNotNull(defaultFilename);

        Template = template;
        GenerationEnvironment = generationEnvironment;
        Model = model;
        DefaultFilename = defaultFilename;
        AdditionalParameters = additionalParameters;
        Context = context;
    }

    public object Template { get; }

    public IGenerationEnvironment GenerationEnvironment { get; }

    public string DefaultFilename { get; }

    public TModel? Model { get; }

    public object? AdditionalParameters { get; }

    public ITemplateContext? Context { get; }
}
