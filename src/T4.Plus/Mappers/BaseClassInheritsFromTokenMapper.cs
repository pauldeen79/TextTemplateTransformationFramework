using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(BaseClassInheritsFromDirectiveModel))]
    [DirectivePrefix("baseClassInheritsFrom")]
    public sealed class BaseClassInheritsFromTokenMapper<TState> : ISingleTokenMapper<TState, BaseClassInheritsFromDirectiveModel>, IContextValidatableTokenMapper<TState, BaseClassInheritsFromDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, BaseClassInheritsFromDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.TypeName);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, BaseClassInheritsFromDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new BaseClassInheritsFromToken<TState>(context, model.TypeName);
        }
    }
}
