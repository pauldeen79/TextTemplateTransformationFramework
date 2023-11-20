using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(ModelDirectiveModel))]
    [DirectivePrefix("model")]
    public sealed class ModelTokenMapper<TState> : ISingleTokenMapper<TState, ModelDirectiveModel>, IContextValidatableTokenMapper<TState, ModelDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, ModelDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.TypeName);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, ModelDirectiveModel model)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return context.TokenParserCallback.IsChildTemplate
                ? (ITemplateToken<TState>)new ModelTypeToken<TState>
                (
                    context,
                    model.TypeName.FixTypeName().MakeGenericTypeName(model.GenericParameterTypeName.FixTypeName()),
                    model.UseForRoutingOnly,
                    model.UseForRouting
                )
                : new ParameterToken<TState>
                (
                    context,
                    "Model",
                    model.TypeName.FixTypeName().MakeGenericTypeName(model.GenericParameterTypeName.FixTypeName()),
                    componentModelData: new(browsable: false),
                    omitValueAssignment: true,
                    addPropertySetter: model.AddPropertySetter
                );
        }
    }
}
