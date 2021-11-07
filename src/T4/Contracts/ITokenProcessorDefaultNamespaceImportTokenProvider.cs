using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenProcessorDefaultNamespaceImportTokenProvider<TState>
        where TState : class
    {
        IEnumerable<ITemplateToken<TState>> Get();
    }
}
