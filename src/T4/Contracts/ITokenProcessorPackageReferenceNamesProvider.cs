using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenProcessorPackageReferenceNamesProvider<TState>
        where TState : class
    {
        IEnumerable<string> Get(IEnumerable<ITemplateToken<TState>> templateTokens);
    }
}
