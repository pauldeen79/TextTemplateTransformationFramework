using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenProcessor<TState> : ITokenProcessor<TState>
        where TState : class
    {
        private readonly ITokenProcessor<TState> _baseProcessor;
        private const string Key = "TextTemplateTransformationFramework.T4.Plus.TokenProcessor";

        public TokenProcessor(ITokenProcessor<TState> baseProcessor)
        {
            _baseProcessor = baseProcessor ?? throw new ArgumentNullException(nameof(baseProcessor));
        }

        public TemplateCodeOutput<TState> Process(ITextTemplateProcessorContext<TState> context, IEnumerable<ITemplateToken<TState>> tokens)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            if (context.ContainsKey(Key))
            {
                //escape recursion
                return _baseProcessor.Process(context, tokens);
            }
            context.Add(Key, this);
            try
            {
                var callback = new Callback<TState, ITokenProcessor<TState>>(context, this);
                foreach (var interceptorToken in tokens.GetTemplateTokensFromSections<TState, ITokenProcessorInterceptorToken<TState>>()
                                                       .Reverse().ToArray())
                {
                    var result = interceptorToken.Process(tokens, callback);
                    if (result is not null)
                    {
                        return result;
                    }
                }
                return _baseProcessor.Process(context, tokens);
            }
            finally
            {
                context.Remove(Key);
            }
        }
    }
}
