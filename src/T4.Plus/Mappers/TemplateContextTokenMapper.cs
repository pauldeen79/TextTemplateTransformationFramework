using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateRenderCodeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ViewModelClassFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(TemplateContextDirectiveModel))]
    [DirectivePrefix("templateContext")]
    public sealed class TemplateContextTokenMapper<TState> : IMultipleTokenMapper<TState, TemplateContextDirectiveModel>, IContextValidatableTokenMapper<TState, TemplateContextDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, TemplateContextDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, TemplateContextDirectiveModel model)
        {
            if (model != null)
            {
                if (!model.Override)
                {
                    yield return new TemplateContextFieldToken<TState>(context);
                    yield return new TemplateContextViewModelFieldToken<TState>(context);
                    yield return new TemplateContextToken<TState>(context);
                }

                yield return new InitializeTemplateContextToken<TState>(context);
            }
        }
    }
}
