namespace TextTemplateTransformationFramework.Core;

public class TemplateContext : ITemplateContext
{
    public TemplateContext(object template,
                           object? model = null,
                           object? viewModel = null,
                           ITemplateContext? parentContext = null,
                           int? iterationNumber = null,
                           int? iterationCount = null)
    {
        Guard.IsNotNull(template);

        Template = template;
        Model = model;
        ViewModel = viewModel;
        ParentContext = parentContext;
        IterationNumber = iterationNumber;
        IterationCount = iterationCount;
    }

    public object Template { get; }
    public object? Model { get; }
    public object? ViewModel { get; }
    public ITemplateContext? ParentContext { get; }

    public ITemplateContext RootContext
    {
        get
        {
            ITemplateContext? p = this;
            while (p.ParentContext != null)
            {
                p = p.ParentContext;
            }

            return p;
        }
    }

    public T? GetModelFromContextByType<T>(Predicate<ITemplateContext>? predicate = null)
    {
        ITemplateContext? p = this;
        while (p != null)
        {
            if (p.Model is T t && (predicate is null || predicate(p)))
            {
                return t;
            }

            p = p.ParentContext;
        }

        return default;
    }

    public T? GetViewModelFromContextByType<T>(Predicate<ITemplateContext>? predicate = null)
    {
        ITemplateContext? p = this;
        while (p != null)
        {
            if (p.ViewModel is T t && (predicate is null || predicate(p)))
            {
                return t;
            }

            p = p.ParentContext;
        }

        return default;
    }

    public T? GetContextByType<T>(Predicate<ITemplateContext>? predicate = null)
    {
        ITemplateContext? p = this;
        while (p != null)
        {
            if (p.Template is T t && (predicate is null || predicate(p)))
            {
                return t;
            }

            p = p.ParentContext;
        }

        return default;
    }

    public bool IsRootContext => ParentContext is null;

    public ITemplateContext CreateChildContext(object template,
                                               object? model = null,
                                               object? viewModel = null,
                                               int? iterationNumber = null,
                                               int? iterationCount = null)
        => new TemplateContext
        (
            template: template,
            model: model,
            viewModel: viewModel,
            parentContext: this,
            iterationNumber: iterationNumber,
            iterationCount: iterationCount
        );

    public int? IterationNumber { get; set; }
    public int? IterationCount { get; set; }

    public bool HasIterations => IterationNumber is not null && IterationCount is not null;

    public bool? IsFirstIteration
    {
        get
        {
            if (IterationNumber is null || IterationCount is null)
            {
                return null;
            }

            return IterationNumber == 0;
        }
    }

    public bool? IsLastIteration
    {
        get
        {
            if (IterationNumber is null || IterationCount is null)
            {
                return null;
            }

            return IterationNumber + 1 == IterationCount;
        }
    }

    public static ITemplateContext CreateRootContext(object template)
        => new TemplateContext
        (
            template: template
        );
}
