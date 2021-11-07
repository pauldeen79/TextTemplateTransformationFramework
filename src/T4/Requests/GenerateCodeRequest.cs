using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Requests
{
    public class GenerateCodeRequest<TState> where TState : class
    {
        public GenerateCodeRequest(IEnumerable<ITemplateToken<TState>> templateTokens)
        {
            TemplateTokens = templateTokens?.ToArray() ?? Array.Empty<ITemplateToken<TState>>();
        }

        public IEnumerable<ITemplateToken<TState>> TemplateTokens { get; }
    }
}
