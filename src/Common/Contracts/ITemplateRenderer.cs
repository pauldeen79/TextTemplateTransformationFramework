namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateRenderer<TState>
        where TState : class
    {
        ProcessResult Render(ITemplateProcessorContext<TState> context);
    }
}
