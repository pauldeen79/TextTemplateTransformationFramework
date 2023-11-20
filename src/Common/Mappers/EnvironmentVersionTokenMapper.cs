using System;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Models;

namespace TextTemplateTransformationFramework.Common.Mappers
{
    [TokenMapper(typeof(EnvironmentDirectiveModel))]
    [DirectivePrefix("environment")]
    public class EnvironmentVersionTokenMapper<TState> : ISingleTokenMapper<TState, EnvironmentDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, EnvironmentDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new EnvironmentVersionToken<TState>(context, model.Version);
        }
    }
}
