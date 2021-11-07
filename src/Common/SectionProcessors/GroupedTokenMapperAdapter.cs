using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.T4.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.SectionProcessors
{
    public sealed class GroupedTokenMapperAdapter<TState> : IModeledTemplateSectionProcessor<TState>, ITemplateCustomDirectiveName
        where TState : class
    {
        private readonly IEnumerable<TokenMapperAdapter<TState>> _tokenMapperTemplateSectionProcessors;
        private readonly IFileNameProvider _fileNameProvider;
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateCodeCompiler<TState> _templateCodeCompiler;

        public GroupedTokenMapperAdapter(IEnumerable<Type> mapperTypes,
                                         IFileNameProvider fileNameProvider,
                                         IFileContentsProvider fileContentsProvider,
                                         ITemplateCodeCompiler<TState> templateCodeCompiler)
        {
            if (mapperTypes == null)
            {
                throw new ArgumentNullException(nameof(mapperTypes));
            }
            _fileNameProvider = fileNameProvider ?? throw new ArgumentNullException(nameof(fileNameProvider));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));

            _tokenMapperTemplateSectionProcessors = mapperTypes.Select(t => new TokenMapperAdapter<TState>(t, _fileNameProvider, _fileContentsProvider, _templateCodeCompiler)).ToArray();
        }

        public Type ModelType
            => _tokenMapperTemplateSectionProcessors.First().ModelType;

        public string TemplateCustomDirectiveName
            => _tokenMapperTemplateSectionProcessors.First().TemplateCustomDirectiveName;

        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => _tokenMapperTemplateSectionProcessors.Aggregate
            (
                SectionProcessResult<TState>.Empty,
                (seed, processor) => processor.Process(context, seed)
            );

        public override string ToString() => _tokenMapperTemplateSectionProcessors.First().TemplateCustomDirectiveName;
    }
}
