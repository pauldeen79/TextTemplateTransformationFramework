using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(AllowNullExpressionsDirectiveModel))]
    [DirectivePrefix("allowNullExpressions")]
    public sealed class AllowNullExpressionsTokenMapper<TState> : ISingleTokenMapper<TState, AllowNullExpressionsDirectiveModel>, IContextValidatableTokenMapper<TState, AllowNullExpressionsDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, AllowNullExpressionsDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, AllowNullExpressionsDirectiveModel model)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new AllowNullExpressionsToken<TState>(context, GetTemplateClassName(context));
        }

        private static string GetTemplateClassName(SectionContext<TState> context)
            => context.ExistingTokens.GetTemplateTokensFromSections<TState, ITemplateClassNameToken<TState>>()
                                     .Distinct(t => t.ClassName)
                                     .LastOrDefaultWhenEmpty("GeneratedClass");
    }
}
