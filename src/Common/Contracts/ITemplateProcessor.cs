namespace TextTemplateTransformationFramework.Common.Contracts
{
    /// <summary>
    /// Renders a template instance.
    /// </summary>
    public interface ITemplateProcessor<TState>
        where TState : class
    {
        ProcessResult Process(ITextTemplateProcessorContext<TState> context, TemplateCompilerOutput<TState> templateCompilerOutput);
    }
}
