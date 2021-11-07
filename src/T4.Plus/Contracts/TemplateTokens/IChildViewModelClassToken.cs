using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IChildViewModelClassToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string ClassName { get; }
        string BaseClass { get; }
        string ModelType { get; }
        bool CopyPropertiesFromTemplate { get; }
        IEnumerable<ITemplateToken<TState>> ChildTemplateTokens { get; }
    }
}
