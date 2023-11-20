using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(MultipleOutputDirectiveModel))]
    [DirectivePrefix("multipleOutput")]
    public sealed class MultipleOutputTokenMapper<TState> : ISingleTokenMapper<TState, MultipleOutputDirectiveModel>, IContextValidatableTokenMapper<TState, MultipleOutputDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, MultipleOutputDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, MultipleOutputDirectiveModel model)
            => new TemplateFileManagerToken<TState>(context);
    }
}
