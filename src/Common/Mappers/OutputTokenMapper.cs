using System;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Models;

namespace TextTemplateTransformationFramework.Common.Mappers
{
    [TokenMapper(typeof(OutputDirectiveModel))]
    [DirectivePrefix("output")]
    public sealed class OutputTokenMapper<TState> : ISingleTokenMapper<TState, OutputDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, OutputDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new OutputExtensionToken<TState>(context, model.Extension);
        }
    }
}
