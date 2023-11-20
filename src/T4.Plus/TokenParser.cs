using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenParser : ITextTemplateTokenParser<TokenParserState>
    {
        private readonly ITextTemplateTokenParser<TokenParserState> _baseParser;
        private const string Key = "TextTemplateTransformationFramework.T4.Plus.TokenParser";

        public TokenParser(ITextTemplateTokenParser<TokenParserState> baseParser)
        {
            _baseParser = baseParser ?? throw new ArgumentNullException(nameof(baseParser));
        }

        public IEnumerable<ITemplateToken<TokenParserState>> Parse(ITextTemplateProcessorContext<TokenParserState> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ContainsKey(Key))
            {
                //escape recursion
                return _baseParser.Parse(context);
            }
            context.Add(Key, this);
            try
            {
                var result = _baseParser.Parse(context);
                if (!result.OfType<ISourceSectionToken<TokenParserState>>().Any(x => x.SourceSection.StartsWith("@ template", StringComparison.OrdinalIgnoreCase)))
                {
                    result = _baseParser.Parse
                    (
                        new TextTemplateProcessorContext<TokenParserState>
                        (
                            new TextTemplate("<#@ template language=\"C#\" #>"),
                            Array.Empty<TemplateParameter>(),
                            context.Logger,
                            null
                        )
                    ).Concat(result);
                }
                var callback = new Callback<TokenParserState, ITextTemplateTokenParser<TokenParserState>>(context, this);
                foreach (var interceptorToken in result.GetTemplateTokensFromSections<TokenParserState, ITemplateParserInterceptorToken<TokenParserState>>()
                                                       .Reverse().ToArray())
                {
                    var newResult = interceptorToken.Process(context.TextTemplate, context.Parameters, callback);
                    if (newResult is not null)
                    {
                        return newResult;
                    }
                }
                return result;
            }
            finally
            {
                context.Remove(Key);
            }
        }
    }
}
