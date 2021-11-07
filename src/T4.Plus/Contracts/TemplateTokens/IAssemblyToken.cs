using System.Reflection;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IAssemblyToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }
        Assembly Assembly { get; }
    }
}
