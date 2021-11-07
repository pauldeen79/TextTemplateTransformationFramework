using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Mappers
{
    [TokenMapper(typeof(IncludeDirectiveModel))]
    [DirectivePrefix("include")]
    public sealed class IncludeTokenMapper<TState> : IMultipleTokenMapper<TState, IncludeDirectiveModel>, IContextValidatableTokenMapper<TState, IncludeDirectiveModel>, IFileContentsProviderContainer
        where TState : class
    {
        public IFileContentsProvider FileContentsProvider { get; set; }

        public bool IsValidForProcessing(SectionContext<TState> context, IncludeDirectiveModel model)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !(model.Once && context.ExistingTokens.HasTemplateTokenInSections<TState, IIncludeFileToken<TState>>(t => t.IncludeFileName == model.File));
        }

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, IncludeDirectiveModel model)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return context.TokenParserCallback
                .Parse
                (
                    new TextTemplateProcessorContext<TState>
                    (
                        new TextTemplate
                        (
                            FileContentsProvider.GetFileContents(model.File),
                            model.File
                        ),
                        Array.Empty<TemplateParameter>(),
                        context.Logger
                    )
                )
                .Concat(new IncludeFileToken<TState>(context, model.File));
        }
    }
}
