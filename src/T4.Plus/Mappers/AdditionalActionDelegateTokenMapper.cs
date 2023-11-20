using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(AdditionalActionDelegateDirectiveModel))]
    [DirectivePrefix("additionalActionDelegate")]
    public sealed class AdditionalActionDelegateTokenMapper<TState> : ISingleTokenMapper<TState, AdditionalActionDelegateDirectiveModel>, IContextValidatableTokenMapper<TState, AdditionalActionDelegateDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, AdditionalActionDelegateDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, AdditionalActionDelegateDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new CallAdditionalActionDelegateToken<TState>(context, model.SkipInitializationCode);
        }
    }
}
