using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers.Assembly
{
    [GroupedTokenMapper(typeof(AssemblyDirectiveModel))]
    [DirectivePrefix("assembly")]
    public sealed class Reference<TState> : ISingleTokenMapper<TState, AssemblyDirectiveModel<TState>>, IContextValidatableTokenMapper<TState, AssemblyDirectiveModel<TState>>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, AssemblyDirectiveModel<TState> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.Name) && model.FrameworkFilter.IsValidFrameworkVersion();
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, AssemblyDirectiveModel<TState> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new ReferenceToken<TState>(context, model.Name);
        }
    }
}
