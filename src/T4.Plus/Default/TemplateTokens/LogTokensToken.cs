using System;
using System.Collections.Generic;
using CrossCutting.Utilities.ObjectDumper.Extensions;
using CrossCutting.Utilities.ObjectDumper.Parts.Filters;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class LogTokensToken<TState> : TemplateToken<TState>, ITemplateParserInterceptorToken<TState>, ILogTokensToken<TState>
        where TState : class
    {
        public LogTokensToken(SectionContext<TState> context)
            : base(context)
        {
        }

        public IEnumerable<ITemplateToken<TState>> Process(TextTemplate textTemplate,
                                                           IEnumerable<TemplateParameter> parameters,
                                                           ICallback<TState, ITextTemplateTokenParser<TState>> callback)
        {
            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var result = callback.Instance.Parse(callback.Context);
            callback.Context.Logger.Log(result.Dump
            (
                new PropertyNameExclusionFilter(nameof(SectionContext), t => typeof(ITemplateToken<TState>).IsAssignableFrom(t)),
                new PropertyNameExclusionFilter(nameof(FileName), t => typeof(ITemplateToken<TState>).IsAssignableFrom(t)),
                new PropertyNameExclusionFilter(nameof(LineNumber), t => typeof(ITemplateToken<TState>).IsAssignableFrom(t))
            ));
            return result;
        }
    }
}
