using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers.RegisterPlaceholder
{
    [GroupedTokenMapper(typeof(PlaceholderDirectiveModel))]
    [DirectivePrefix("registerPlaceholder")]
    [PassThrough]
    [RootTemplate]
    public sealed class Class<TState> : ISingleTokenMapper<TState, PlaceholderDirectiveModel<TState>>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, PlaceholderDirectiveModel<TState> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new PlaceholderClassToken<TState>
            (
                context,
                model.Name,
                model.BaseClass.WhenNullOrEmpty(context.GetRootClassName()),
                context.GetRootClassName(),
                model.ModelTypeName
            );
        }
    }
}
