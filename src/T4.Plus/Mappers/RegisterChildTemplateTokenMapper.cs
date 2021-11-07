using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(RegisterChildTemplateDirectiveModel))]
    [DirectivePrefix("registerChildTemplate")]
    public sealed class RegisterChildTemplateTokenMapper<TState> : RegisterChildTemplateTokenMapperBase<TState, RegisterChildTemplateDirectiveModel<TState>>
        where TState : class
    {
        protected override bool GetIsViewModel() => false;
    }
}
