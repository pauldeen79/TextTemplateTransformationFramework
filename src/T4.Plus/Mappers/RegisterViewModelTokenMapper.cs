using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(RegisterViewModelDirectiveModel))]
    [DirectivePrefix("registerViewModel")]
    public sealed class RegisterViewModelTokenMapper<TState> : RegisterChildTemplateTokenMapperBase<TState, RegisterViewModelDirectiveModel<TState>>
        where TState : class
    {
        protected override bool GetIsViewModel() => true;
    }
}
