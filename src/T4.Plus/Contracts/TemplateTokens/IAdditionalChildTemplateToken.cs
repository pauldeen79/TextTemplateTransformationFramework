using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IAdditionalChildTemplateToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string TemplateName { get; }
        Func<object> TemplateDelegate { get; }
        Type ModelType { get; }
    }
}
