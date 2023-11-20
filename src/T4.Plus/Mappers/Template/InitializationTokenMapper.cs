using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Models;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers.Template
{
    [GroupedTokenMapper(typeof(TemplateDirectiveModel))]
    [DirectivePrefix("template")]
    [PassThrough]
    public sealed class InitializationTokenMapper<TState> : IMultipleTokenMapper<TState, TemplateDirectiveModel>
        where TState : class
    {
        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, TemplateDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return string.IsNullOrEmpty(model.BaseClassName)
                ? new ITemplateToken<TState>[]
                {
                    new ClearErrorsToken<TState>(context),
                    new ClearGenerationEnvironmentToken<TState>(context),
                    new EnsureSessionInitializedToken<TState>(context),
                }
                : Array.Empty<ITemplateToken<TState>>();
        }
    }
}
