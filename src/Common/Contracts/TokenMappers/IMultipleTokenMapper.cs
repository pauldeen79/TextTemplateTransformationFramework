using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts.TokenMappers
{
    public interface IMultipleTokenMapper<TState, TModel> : ITokenMapper
        where TState : class
    {
        IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, TModel model);
    }
}
