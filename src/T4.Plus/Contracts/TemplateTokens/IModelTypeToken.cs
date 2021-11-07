using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IModelTypeToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string ModelTypeName { get; }
        bool UseForRoutingOnly { get; }
        bool UseForRouting { get; }
    }
}
