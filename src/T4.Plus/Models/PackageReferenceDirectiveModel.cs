using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Adds a NuGet package reference")]
    public class PackageReferenceDirectiveModel
    {
        [Description("Package name")]
        [Required]
        public string Name { get; set; }

        [Description("Package version")]
        [Required]
        public string Version { get; set; }

        [Description("Framework name (i.e. .NETStandard,Version=v2.0)")]
        [DefaultValue(".NETStandard,Version=v2.0")]
        public string FrameworkVersion { get; set; }

        [Description("Optional framework name (for example .NETCoreApp or .NETFramework) that the current entry assembly should start with")]
        public string FrameworkFilter { get; set; }
    }
}
