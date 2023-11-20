using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(WarningDirectiveModel))]
    [DirectivePrefix("warning")]
    public sealed class WarningTokenMapper<TState> : ISingleTokenMapper<TState, WarningDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, WarningDirectiveModel model)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return context.CreateWarningToken(model.Message);
        }
    }
}
