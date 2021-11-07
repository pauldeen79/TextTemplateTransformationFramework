using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.Interception
{
    public class TemplateParserInterceptorToken<TState> : TemplateToken<TState>, ITemplateParserInterceptorToken<TState>
        where TState : class
    {
        private readonly Func<TextTemplate, IEnumerable<TemplateParameter>, ICallback<TState, ITextTemplateTokenParser<TState>>, IEnumerable<ITemplateToken<TState>>> _processDelegate;

        public TemplateParserInterceptorToken(SectionContext<TState> context, Func<TextTemplate, IEnumerable<TemplateParameter>, ICallback<TState, ITextTemplateTokenParser<TState>>, IEnumerable<ITemplateToken<TState>>> processDelegate)
            : base(context)
            => _processDelegate = processDelegate ?? throw new ArgumentNullException(nameof(processDelegate));

        public IEnumerable<ITemplateToken<TState>> Process(TextTemplate textTemplate, IEnumerable<TemplateParameter> parameters, ICallback<TState, ITextTemplateTokenParser<TState>> callback)
            => _processDelegate(textTemplate, parameters, callback);
    }
}
