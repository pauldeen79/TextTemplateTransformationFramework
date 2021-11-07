using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class PlaceholderProcessorToken<TState> : TemplateToken<TState>, IPlaceholderProcessorToken<TState>
        where TState : class
    {
        private Func<string, IEnumerable<ITemplateToken<TState>>, string> ProcessDelegate { get; }

        public PlaceholderProcessorToken(SectionContext<TState> context, Func<string, IEnumerable<ITemplateToken<TState>>, string> processDelegate)
            : base(context)
        {
            ProcessDelegate = processDelegate;
        }

        public string Process(string value, IEnumerable<ITemplateToken<TState>> existingTokens)
            => ProcessDelegate(value, existingTokens);
    }
}
