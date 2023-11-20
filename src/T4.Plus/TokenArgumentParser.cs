using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenArgumentParser<TState> : ITokenArgumentParser<TState>
        where TState : class
    {
        public IEnumerable<string> ParseArgument(SectionContext<TState> context, string argumentName)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.ExistingTokens
                .GetTemplateTokensFromSections<TState, ITokenParserInterceptorToken<TState>>()
                .SelectMany(i => i.Parse(context, argumentName));
        }
    }
}
