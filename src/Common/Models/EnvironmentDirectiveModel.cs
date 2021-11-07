using System.ComponentModel;

namespace TextTemplateTransformationFramework.Common.Models
{
    [Description("Sets environment properties")]
    public class EnvironmentDirectiveModel
    {
        [Description("Version of the environment. When null, then the Environment.Version is used, which represents the framework version")]
        public string Version { get; set; }
    }
}
