using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateOutputCreator<TState>
        where TState : class
    {
        TemplateCodeOutput<TState> Create(ITextTemplateProcessorContext<TState> context);
    }
}
