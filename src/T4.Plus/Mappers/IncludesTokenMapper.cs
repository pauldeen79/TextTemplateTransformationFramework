using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Models;
using CommonIncludeDirectiveModel = TextTemplateTransformationFramework.Common.Models.IncludeDirectiveModel;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(IncludesDirectiveModel))]
    [DirectivePrefix("includes")]
    public sealed class IncludesTokenMapper<TState> : IMultipleTokenMapper<TState, IncludesDirectiveModel>, IFileNameProviderContainer, IFileContentsProviderContainer
        where TState : class
    {
        public IFileNameProvider FileNameProvider { get; set; }
        public IFileContentsProvider FileContentsProvider { get; set; }

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, IncludesDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return FileNameProvider
                .GetFiles(model.Path, model.SearchPattern, model.Recurse)
                .Select
                (
                    fileName => new CommonIncludeDirectiveModel
                    {
                        File = fileName,
                        Once = model.Once
                    }
                )
                .Where(commonModel => new TextTemplateTransformationFramework.Common.Mappers.IncludeTokenMapper<TState> { FileContentsProvider = FileContentsProvider }.IsValidForProcessing(context, commonModel))
                .SelectMany
                (
                    commonModel => new TextTemplateTransformationFramework.Common.Mappers.IncludeTokenMapper<TState> { FileContentsProvider = FileContentsProvider }
                        .Map
                        (
                            context,
                            commonModel
                        )
                );
        }
    }
}
