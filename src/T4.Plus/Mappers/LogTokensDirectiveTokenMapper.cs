using System;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(LogTokensDirectiveModel))]
    [DirectivePrefix("logTokens")]
    public sealed class LogTokensTokenMapper<TState> : ISingleTokenMapper<TState, LogTokensDirectiveModel>, IContextValidatableTokenMapper<TState, LogTokensDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, LogTokensDirectiveModel model)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled && !context.ExistingTokens.GetTemplateTokensFromSections<TState, ILogTokensToken<TState>>().Any();
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, LogTokensDirectiveModel model)
            => new LogTokensToken<TState>(context);
    }
}
