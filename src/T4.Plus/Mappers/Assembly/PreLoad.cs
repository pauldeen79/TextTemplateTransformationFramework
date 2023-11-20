using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers.Assembly
{
    [GroupedTokenMapper(typeof(AssemblyDirectiveModel))]
    [DirectivePrefix("assembly")]
    public sealed class PreLoad<TState> : ISingleTokenMapper<TState, AssemblyDirectiveModel<TState>>, IContextValidatableTokenMapper<TState, AssemblyDirectiveModel<TState>>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, AssemblyDirectiveModel<TState> model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.Name) && model.PreLoad;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, AssemblyDirectiveModel<TState> model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new PreLoadToken<TState>(context, model.HintPath.WhenNullOrEmpty(model.Name));
        }
    }
}
