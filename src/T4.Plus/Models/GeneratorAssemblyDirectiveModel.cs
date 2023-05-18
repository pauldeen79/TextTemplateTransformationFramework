using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Runs code generation providers from assembly")]
    public class GeneratorAssemblyDirectiveModel
    {
        [Description("Assembly name")]
        [Required]
        public string AssemblyName { get; set; }

        [Description("Indicator whether the assembly name is a literal expression; true by default")]
        [DefaultValue(true)]
        public bool AssemblyNameIsLiteral { get; set; }

        [Description("Base path for output")]
        public string BasePath { get; set; }

        [Description("Indicator whether the base path is a literal expression; true by default")]
        [DefaultValue(true)]
        public bool BasePathIsLiteral { get; set; }

        [Description("Indicator that defines whether multiple files should be generated")]
        public bool GenerateMultipleFiles { get; set; }

        [Description("Indicator that defines whether a dry run should be performed, skipping output to files")]
        public bool DryRun { get; set; }

        [Description("Optional directory to use for loading dependent assemblies")]
        public string CurrentDirectory { get; set; }

        [Description("Indicator whether the current directory is a literal expression; true by default")]
        [DefaultValue(true)]
        public bool CurrentDirectoryIsLiteral { get; set; }
    }
}
