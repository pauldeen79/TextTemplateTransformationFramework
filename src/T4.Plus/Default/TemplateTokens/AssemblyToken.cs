using System.Reflection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class AssemblyToken<TState> : TemplateToken<TState>, IAssemblyToken<TState>
        where TState : class
    {
        public AssemblyToken(SectionContext<TState> context, string name, Assembly assembly)
            : base(context)
        {
            Name = name;
            Assembly = assembly;
        }

        public string Name { get; }
        public Assembly Assembly { get; }
    }
}
