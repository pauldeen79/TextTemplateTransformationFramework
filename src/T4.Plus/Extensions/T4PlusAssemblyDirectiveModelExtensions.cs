using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Extensions
{
    public static class T4PlusAssemblyDirectiveModelExtensions
    {
        public static IHintPathToken<TState> GetHintPathToken<TState>(this AssemblyDirectiveModel<TState> instance)
            where TState : class
        {
            if (!string.IsNullOrEmpty(instance.Name))
            {
                if (!string.IsNullOrEmpty(instance.HintPath))
                {
                    return new HintPathToken<TState>(instance.SectionContext, instance.Name, instance.HintPath, instance.Recursive);
                }
                else if (instance.Name.IsFullyQualifiedAssemblyName())
                {
                    return new HintPathToken<TState>(instance.SectionContext, instance.Name, instance.Name.GetAssemblyName(), instance.Recursive);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return new HintPathToken<TState>(instance.SectionContext, instance.Name, instance.HintPath, instance.Recursive);
            }
        }
    }
}
