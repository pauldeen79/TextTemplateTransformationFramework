using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IParameterPropertiesToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
