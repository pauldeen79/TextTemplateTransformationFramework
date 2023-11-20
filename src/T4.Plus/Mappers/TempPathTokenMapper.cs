using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(TempPathDirectiveModel))]
    [DirectivePrefix("tempPath")]
    public sealed class TempPathTokenMapper<TState> : ISingleTokenMapper<TState, TempPathDirectiveModel>, IContextValidatableTokenMapper<TState, TempPathDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, TempPathDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.Value);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, TempPathDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new TempPathToken<TState>(context, model.Value);
        }
    }
}
