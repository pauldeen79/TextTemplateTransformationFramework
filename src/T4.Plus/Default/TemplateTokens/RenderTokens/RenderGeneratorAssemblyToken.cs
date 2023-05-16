using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens
{
    public class RenderGeneratorAssemblyToken<TState> : TemplateToken<TState>, IRenderGeneratorAssemblyToken<TState>
        where TState : class
    {
        public RenderGeneratorAssemblyToken(SectionContext<TState> context, string assemblyName)
            : base(context)
        {
            AssemblyName = assemblyName;
        }

        public string AssemblyName { get; }
    }
}
