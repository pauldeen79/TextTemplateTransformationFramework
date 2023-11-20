using System;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Models;

namespace TextTemplateTransformationFramework.Common.Mappers
{
    [TokenMapper(typeof(AssemblyDirectiveModel))]
    [DirectivePrefix("assembly")]
    public sealed class AssemblyTokenMapper<TState> : ISingleTokenMapper<TState, AssemblyDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, AssemblyDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new ReferenceToken<TState>(context, model.Name);
        }
    }
}
