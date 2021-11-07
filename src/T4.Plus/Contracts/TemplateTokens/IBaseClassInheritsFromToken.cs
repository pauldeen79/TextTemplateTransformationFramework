using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IBaseClassInheritsFromToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string ClassName { get; }
    }
}
