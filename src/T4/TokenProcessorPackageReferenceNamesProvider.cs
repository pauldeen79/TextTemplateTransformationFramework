using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenProcessorPackageReferenceNamesProvider<TState> : ITokenProcessorPackageReferenceNamesProvider<TState>
        where TState : class
    {
        public IEnumerable<string> Get(IEnumerable<ITemplateToken<TState>> templateTokens)
            => templateTokens
                .OfType<IPackageReferenceToken<TState>>()
                .Select(CreatePackageReferenceString)
                .Distinct();

        private static string CreatePackageReferenceString(IPackageReferenceToken<TState> t)
            => string.IsNullOrEmpty(t.FrameworkVersion)
                ? $"{t.Name},{t.Version}"
                : $"{t.Name},{t.Version},{t.FrameworkVersion}";
    }
}
