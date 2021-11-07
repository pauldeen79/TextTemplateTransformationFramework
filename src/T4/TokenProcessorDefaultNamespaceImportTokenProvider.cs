using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    internal static class TokenProcessorDefaultNamespaceImportTokenProvider
    {
        internal static readonly string[] DefaultNamespaces = new[]
        {
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text"
        };

    }
    public class TokenProcessorDefaultNamespaceImportTokenProvider<TState> : ITokenProcessorDefaultNamespaceImportTokenProvider<TState>
        where TState : class
    {
       
        public IEnumerable<ITemplateToken<TState>> Get()
            => TokenProcessorDefaultNamespaceImportTokenProvider.DefaultNamespaces.Select(ns => new NamespaceImportToken<TState>(SectionContext<TState>.Empty, ns));
    }
}
