using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Extensions
{
    public static class ParameterTokenExtensions
    {
        public static TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.IParameterToken<TState> WithPropertySetter<TState>(this TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.IParameterToken<TState> instance)
            where TState : class
            => new ParameterToken<TState>
            (
                instance.SectionContext,
                instance.Name,
                instance.TypeName,
                instance.NetCoreCompatible,
                addPropertySetter: true,
                componentModelData: new(browsable: false)
            );
    }
}
