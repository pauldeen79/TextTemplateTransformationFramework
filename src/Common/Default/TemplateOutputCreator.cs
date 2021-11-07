using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateOutputCreator<TState> : ITemplateOutputCreator<TState>
        where TState : class
    {
        private readonly ITokenProcessor<TState> _tokenProcessor;
        private readonly ITextTemplateTokenParser<TState> _textTemplateTokenParser;

        public TemplateOutputCreator(ITokenProcessor<TState> tokenProcessor,
                                     ITextTemplateTokenParser<TState> textTemplateTokenParser)
        {
            _tokenProcessor = tokenProcessor ?? throw new ArgumentNullException(nameof(tokenProcessor));
            _textTemplateTokenParser = textTemplateTokenParser ?? throw new ArgumentNullException(nameof(textTemplateTokenParser));
        }

        public TemplateCodeOutput<TState> Create(ITextTemplateProcessorContext<TState> context)
        {
            // Parse template into tokens
            var tokens = ParseTextTemplateTokens(context).ToArray();

            // Convert tokens into code output
            var codeOutput = ProcessTokens(context, tokens);

            return new TemplateCodeOutput<TState>
            (
                tokens,
                codeOutput.SourceCode,
                codeOutput
            );
        }

        private IEnumerable<ITemplateToken<TState>> ParseTextTemplateTokens(ITextTemplateProcessorContext<TState> context)
            => _textTemplateTokenParser.Parse(context);

        private TemplateCodeOutput<TState> ProcessTokens(ITextTemplateProcessorContext<TState> context, IEnumerable<ITemplateToken<TState>> tokens)
            => _tokenProcessor.Process(context, tokens);
    }
}
