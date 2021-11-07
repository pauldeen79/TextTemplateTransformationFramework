namespace TextTemplateTransformationFramework.Common.Contracts.TokenMappers
{
    public interface ISingleTokenMapper<TState, TModel> : ITokenMapper
        where TState : class
    {
        ITemplateToken<TState> Map(SectionContext<TState> context, TModel model);
    }
}
