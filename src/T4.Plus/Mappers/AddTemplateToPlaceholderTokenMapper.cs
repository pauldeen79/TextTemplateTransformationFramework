using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(AddTemplateToPlaceholderDirectiveModel))]
    [DirectivePrefix("addToPlaceholder")]
    public sealed class AddTemplateToPlaceholderTokenMapper<TState> : ISingleTokenMapper<TState, AddTemplateToPlaceholderDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, AddTemplateToPlaceholderDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new InitializeAddTemplateToPlaceholderToken<TState>
            (
                context,
                model.Name,
                model.NameIsLiteral,
                model.ChildTemplateName,
                model.ChildTemplateNameIsLiteral
            );
        }
    }
}
