namespace TextTemplateTransformationFramework.Common.Contracts.TokenMappers
{
    public interface IContextValidatableTokenMapper<TState, TModel>
        where TState : class
    {
        bool IsValidForProcessing(SectionContext<TState> context, TModel model);
    }
}
