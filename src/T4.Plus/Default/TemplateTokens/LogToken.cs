using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class LogToken<TState> : TemplateToken<TState>, ILogToken<TState>, ITemplateParserInterceptorToken<TState>
        where TState : class
    {
        public string Message { get; }

        public LogToken(SectionContext<TState> context, string message) : base(context)
            => Message = message;

        public IEnumerable<ITemplateToken<TState>> Process(TextTemplate textTemplate, IEnumerable<TemplateParameter> parameters, ICallback<TState, ITextTemplateTokenParser<TState>> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            callback.Context.Logger.Log(Message);
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
        }
    }
}
