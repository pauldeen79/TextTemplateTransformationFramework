namespace TextTemplateTransformationFramework.Abstractions
{
    public interface ITemplateContext
    {
        object Template { get; }
        object? Model { get; set; }
        object? ViewModel { get; set; }
        ITemplateContext? ParentContext { get; }
        ITemplateContext RootContext { get; }
        bool IsRootContext { get; }
        int? IterationNumber { get; set; }
        int? IterationCount { get; set; }
        bool? IsFirstIteration { get; }
        bool? IsLastIteration { get; }

        T? GetModelFromContextByType<T>(Predicate<ITemplateContext>? predicate = null);
        T? GetViewModelFromContextByType<T>(Predicate<ITemplateContext>? predicate = null);
        T? GetContextByType<T>(Predicate<ITemplateContext>? predicate = null);

        ITemplateContext CreateChildContext(object template,
                                            object? model = null,
                                            object? viewModel = null,
                                            int? iterationNumber = null,
                                            int? iterationCount = null);
    }
}
