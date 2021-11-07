namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateInitializeParameterSetter<TState>
        where TState : class
    {
        void Set(ITemplateProcessorContext<TState> context);
    }
}
