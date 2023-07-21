namespace TemplateFramework.Core.Requests;

public class RenderTemplateRequest : IRenderTemplateRequest
{
    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), defaultFilename, additionalParameters, context)
    {
    }

    protected RenderTemplateRequest(
        object template,
        IGenerationEnvironment generationEnvironment,
        string defaultFilename,
        object? additionalParameters,
        ITemplateContext? context)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(defaultFilename);

        Template = template;
        GenerationEnvironment = generationEnvironment;
        DefaultFilename = defaultFilename;
        AdditionalParameters = additionalParameters;
        Context = context;
    }

    public object Template { get; }

    public IGenerationEnvironment GenerationEnvironment { get; }

    public string DefaultFilename { get; }

    public object? AdditionalParameters { get; }

    public ITemplateContext? Context { get; }
}

public class RenderTemplateRequest<TModel> : RenderTemplateRequest, IRenderTemplateRequest<TModel>
{
    public RenderTemplateRequest(IRenderTemplateRequest request)
        : this((request ?? throw new ArgumentNullException(nameof(request))).Template, request.GenerationEnvironment, request.DefaultFilename, default, request.AdditionalParameters, request.Context)
    {
    }

    public RenderTemplateRequest(
        object template,
        StringBuilder builder,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new StringBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, model, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilder builder,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderEnvironment(builder ?? throw new ArgumentNullException(nameof(builder))), defaultFilename, model, additionalParameters, context)
    {
    }

    public RenderTemplateRequest(
        object template,
        IMultipleContentBuilderContainer builderContainer,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context)
        : this(template, new MultipleContentBuilderContainerEnvironment(builderContainer ?? throw new ArgumentNullException(nameof(builderContainer))), defaultFilename, model, additionalParameters, context)
    {
    }

    private RenderTemplateRequest(
        object template,
        IGenerationEnvironment generationEnvironment,
        string defaultFilename,
        TModel? model,
        object? additionalParameters,
        ITemplateContext? context) : base(template, generationEnvironment, defaultFilename, additionalParameters, context)
    {
        Model = model;
    }

    public TModel? Model { get; }
}
