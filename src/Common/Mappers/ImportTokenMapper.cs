using System;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Models;

namespace TextTemplateTransformationFramework.Common.Mappers
{
    [TokenMapper(typeof(ImportDirectiveModel))]
    [DirectivePrefix("import")]
    public sealed class ImportTokenMapper<TState> : ISingleTokenMapper<TState, ImportDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, ImportDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new NamespaceImportToken<TState>(context, model.Namespace);
        }
    }
}
