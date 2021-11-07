using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(RegisterViewModelDirectoryDirectiveModel))]
    [DirectivePrefix("registerViewModels")]
    public sealed class RegisterViewModelDirectoryTokenMapper<TState> : RegisterChildTemplateDirectoryTokenMapperBase<TState, RegisterViewModelDirectoryDirectiveModel<TState>>
        where TState : class
    {
        protected override bool GetIsViewModel() => true;
    }
}
