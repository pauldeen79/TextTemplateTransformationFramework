using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(ClearChildTemplatesFieldModel))]
    [DirectivePrefix("clearChildTemplatesField")]
    public sealed class ClearChildTemplatesFieldTokenMapper<TState> : ISingleTokenMapper<TState, ClearChildTemplatesFieldModel>, IContextValidatableTokenMapper<TState, ClearChildTemplatesFieldModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, ClearChildTemplatesFieldModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, ClearChildTemplatesFieldModel model)
            => new ClearChildTemplatesFieldToken<TState>(context);
    }
}
