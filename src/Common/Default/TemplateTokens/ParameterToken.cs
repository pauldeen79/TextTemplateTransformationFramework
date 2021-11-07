using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class ParameterToken<TState> : TemplateToken<TState>, IParameterToken<TState>
        where TState : class
    {
        public ParameterToken(SectionContext<TState> context, string name, string typeName, bool netCoreCompatible = true)
            : base(context)
        {
            Name = name;
            TypeName = typeName;
            NetCoreCompatible = netCoreCompatible;
        }

        public string Name { get; }
        public string TypeName { get; }
        public bool NetCoreCompatible { get; }
    }
}
