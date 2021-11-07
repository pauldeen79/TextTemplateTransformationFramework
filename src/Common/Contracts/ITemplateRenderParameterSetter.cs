namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateRenderParameterSetter<TState>
        where TState : class
    {
        void Set(ITemplateProcessorContext<TState> context);
    }
}
