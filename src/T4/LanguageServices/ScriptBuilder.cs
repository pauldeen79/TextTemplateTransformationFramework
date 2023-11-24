using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.LanguageServices;
using TextTemplateTransformationFramework.Common.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.LanguageServices
{
    public sealed class ScriptBuilder : IScriptBuilder<TokenParserState>
    {
        private readonly IEnumerable<ITemplateSectionProcessor<TokenParserState>> _knownDirectives;
        private readonly IFileNameProvider _fileNameProvider;
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateCodeCompiler<TokenParserState> _templateCodeCompiler;

        public ScriptBuilder(ITemplateSectionProcessor<TokenParserState> defaultSectionProcessor,
                             IFileNameProvider fileNameProvider,
                             IFileContentsProvider fileContentsProvider,
                             ITemplateCodeCompiler<TokenParserState> templateCodeCompiler)
        {
            if (defaultSectionProcessor == null)
            {
                throw new ArgumentNullException(nameof(defaultSectionProcessor));
            }

            _fileNameProvider = fileNameProvider ?? throw new ArgumentNullException(nameof(fileNameProvider));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));

            _knownDirectives = defaultSectionProcessor
                .GetContainedTemplateSectionProcessors()
                .DistinctBy(d => d.GetDirectiveName())
                .ToArray();
        }

        public IEnumerable<ITemplateSectionProcessor<TokenParserState>> GetKnownDirectives()
            => _knownDirectives;

        public string Build(ITemplateSectionProcessor<TokenParserState> templateSectionProcessor, params object[] models)
        {
            var stringBuilder = new StringBuilder();

            foreach (var model in models ?? Array.Empty<object>())
            {
                var serializerType = typeof(DirectiveSerializer<,>).MakeGenericType(typeof(TokenParserState), model.GetType());
                var context = SectionContext<TokenParserState>.Empty;
                var serializer = Activator.CreateInstance(serializerType, context, _fileNameProvider, _fileContentsProvider, _templateCodeCompiler);
                var serializerOutput = model is SectionModel sm
                    ? sm.Code ?? string.Empty
                    : serializerType
                        .GetMethod(nameof(DirectiveSerializer<TokenParserState, object>.Serialize))
                        .Invoke(serializer, [model])
                        .ToStringWithDefault(string.Empty);

                if (stringBuilder.Length > 0)
                {
                    stringBuilder.AppendLine();
                }

                stringBuilder
                    .Append(templateSectionProcessor.IsDirective() ? "<#@ " : "<#")
                    .Append(templateSectionProcessor.GetDirectivePrefix().ToCamelCase())
                    .Append(" ")
                    .Append(serializerOutput)
                    .Append(serializerOutput.Length > 0
                        ? " "
                        : string.Empty)
                    .Append("#>");
            }

            return stringBuilder.ToString();
        }
    }
}
