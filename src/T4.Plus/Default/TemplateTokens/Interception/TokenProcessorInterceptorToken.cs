using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.Interception
{
    public class TokenProcessorInterceptorToken<TState> : TemplateToken<TState>, ITokenProcessorInterceptorToken<TState>
        where TState : class
    {
        private readonly Func<IEnumerable<ITemplateToken<TState>>, ICallback<TState, ITokenProcessor<TState>>, TemplateCodeOutput<TState>> _processDelegate;

        public TokenProcessorInterceptorToken(SectionContext<TState> context, Func<IEnumerable<ITemplateToken<TState>>, ICallback<TState, ITokenProcessor<TState>>, TemplateCodeOutput<TState>> processDelegate)
            : base(context)
            => _processDelegate = processDelegate ?? throw new ArgumentNullException(nameof(processDelegate));

        public TemplateCodeOutput<TState> Process(IEnumerable<ITemplateToken<TState>> tokens, ICallback<TState, ITokenProcessor<TState>> callback)
            => _processDelegate(tokens, callback);
    }
}
