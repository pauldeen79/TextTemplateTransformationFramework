using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4
{
    internal static class Skip
    {
        internal static readonly string[] Assemblies =
        [
            "mscorlib.dll",
            "system.core"
        ];
    }

    public class TokenProcessorReferenceAssemblyNamesProvider<TState> : ITokenProcessorReferenceAssemblyNamesProvider<TState>
        where TState : class
    {
        public IEnumerable<string> Get(IEnumerable<ITemplateToken<TState>> templateTokens)
            => templateTokens
                .OfType<IReferenceToken<TState>>()
                .Select(t => t.Name)
                .Where(n => !n.In(StringComparison.OrdinalIgnoreCase, Skip.Assemblies))
                .Distinct();
    }
}
