using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(SkipInitializationCodeDirectiveModel))]
    [DirectivePrefix("skipInitializationCode")]
    public sealed class SkipInitializationCodeTokenMapper<TState> : ISingleTokenMapper<TState, SkipInitializationCodeDirectiveModel>, IContextValidatableTokenMapper<TState, SkipInitializationCodeDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, SkipInitializationCodeDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, SkipInitializationCodeDirectiveModel model)
            => new SkipInitializationCodeToken<TState>(context);
    }
}
