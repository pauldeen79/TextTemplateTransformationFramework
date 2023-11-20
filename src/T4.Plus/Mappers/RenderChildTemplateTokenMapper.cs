using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(RenderChildTemplateDirectiveModel))]
    [DirectivePrefix("renderChildTemplate")]
    public sealed class RenderChildTemplate<TState> : ISingleTokenMapper<TState, RenderChildTemplateDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, RenderChildTemplateDirectiveModel model)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return context.CreateRenderChildTemplateToken(model);
        }
    }
}
