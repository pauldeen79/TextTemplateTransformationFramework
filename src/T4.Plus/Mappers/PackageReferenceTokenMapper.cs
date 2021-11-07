using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(PackageReferenceDirectiveModel))]
    [DirectivePrefix("packageReference")]
    public sealed class PackageReferenceTokenMapper<TState> : ISingleTokenMapper<TState, PackageReferenceDirectiveModel>, IContextValidatableTokenMapper<TState, PackageReferenceDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, PackageReferenceDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.Name) && model.FrameworkFilter.IsValidFrameworkVersion();
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, PackageReferenceDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new PackageReferenceToken<TState>(context, model.Name, model.Version, model.FrameworkVersion, model.FrameworkFilter);
        }
    }
}
