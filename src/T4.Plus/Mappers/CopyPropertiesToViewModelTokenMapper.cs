using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(CopyPropertiesToViewModelDirectiveModel))]
    [DirectivePrefix("copyPropertiesToViewModel")]
    public sealed class CopyPropertiesToViewModelTokenMapper<TState> : ISingleTokenMapper<TState, CopyPropertiesToViewModelDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, CopyPropertiesToViewModelDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            return new CopyPropertiesToViewModelToken<TState>(context, model.Enabled);
        }
    }
}
