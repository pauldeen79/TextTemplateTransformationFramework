using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class PackageReferenceToken<TState> : TemplateToken<TState>, IPackageReferenceToken<TState>
        where TState : class
    {
        public PackageReferenceToken(SectionContext<TState> context,
                                     string name,
                                     string version,
                                     string frameworkVersion,
                                     string frameworkFilter)
            : base(context)
        {
            Name = name;
            Version = version;
            FrameworkVersion = frameworkVersion;
            FrameworkFilter = frameworkFilter;
        }

        public string Name { get; }

        public string Version { get; }

        public string FrameworkVersion { get; }

        public string FrameworkFilter { get; }
    }
}
