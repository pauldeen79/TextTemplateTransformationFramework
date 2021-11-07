using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class TokenParserInterceptorTokenMock<TState> : TemplateToken<TState>, ITokenParserInterceptorToken<TState>
        where TState : class
    {
        public TokenParserInterceptorTokenMock(SectionContext<TState> context)
            : base(context)
        {
        }

        public IEnumerable<string> Parse(SectionContext<TState> context, string argumentName)
        {
            if (context?.Section.IsDirective("RenderChildTemplate", "@ ", " ") == true
                && argumentName?.Equals("name", StringComparison.OrdinalIgnoreCase) == true)
            {
                yield return "child";
            }
        }
    }
}
