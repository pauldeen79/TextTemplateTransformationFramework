using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts
{
    public interface ICallback<TState, TCallback>
        where TState : class
    {
        TCallback Instance { get; }
        ITextTemplateProcessorContext<TState> Context { get; }
    }
}
