using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(RegisterChildTemplateDirectoryDirectiveModel))]
    [DirectivePrefix("registerChildTemplates")]
    public sealed class RegisterChildTemplateDirectoryTokenMapper<TState> : RegisterChildTemplateDirectoryTokenMapperBase<TState, RegisterChildTemplateDirectoryDirectiveModel<TState>>
        where TState: class
    {
        protected override bool GetIsViewModel() => false;
    }
}
