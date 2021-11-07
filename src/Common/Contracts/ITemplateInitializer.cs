namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateInitializer<TState>
        where TState : class
    {
        void Initialize(ITemplateProcessorContext<TState> context);
    }
}
