using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(GeneratorAssemblyDirectiveModel))]
    [DirectivePrefix("generators")]
    public sealed class GeneratorAssemblyTokenMapper<TState> : ISingleTokenMapper<TState, GeneratorAssemblyDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, GeneratorAssemblyDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new RenderGeneratorAssemblyToken<TState>(context, model.AssemblyName, model.AssemblyNameIsLiteral, model.BasePath, model.BasePathIsLiteral, model.GenerateMultipleFiles, model.DryRun, model.CurrentDirectory, model.CurrentDirectoryIsLiteral);
        }
    }
}
