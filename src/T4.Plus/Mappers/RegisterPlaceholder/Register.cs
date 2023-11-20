using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers.RegisterPlaceholder
{
    [GroupedTokenMapper(typeof(PlaceholderDirectiveModel))]
    [DirectivePrefix("registerPlaceholder")]
    [PassThrough]
    [RootTemplate]
    public sealed class Register<TState> : ISingleTokenMapper<TState, PlaceholderDirectiveModel<TState>>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, PlaceholderDirectiveModel<TState> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new RegisterPlaceholderToken<TState>
            (
                context,
                model.Name,
                model.NameIsLiteral,
                model.ModelTypeName
            );
        }
    }
}
