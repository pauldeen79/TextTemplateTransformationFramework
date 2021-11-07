using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(AddTemplateLineNumbersDirectiveModel))]
    [DirectivePrefix("addTemplateLineNumbers")]
    public sealed class AddTemplateLineNumbersTokenMapper<TState> : ISingleTokenMapper<TState, AddTemplateLineNumbersDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, AddTemplateLineNumbersDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new AddTemplateLineNumbersToken<TState>(context, model.Enabled);
        }
    }
}
