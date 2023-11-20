using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(AddExcludeFromCodeCoverageAttributesDirectiveModel))]
    [DirectivePrefix("addExcludeFromCodeCoverageAttributes")]
    public sealed class AddExcludeFromCodeCoverageAttributesMapper<TState> : ISingleTokenMapper<TState, AddExcludeFromCodeCoverageAttributesDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, AddExcludeFromCodeCoverageAttributesDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new AddExcludeFromCodeCoverageAttributesToken<TState>(context, model.Enabled);
        }
    }
}
