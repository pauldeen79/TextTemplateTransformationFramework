using System.ComponentModel;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Custom type to inherit the base class from")]
    public class BaseClassInheritsFromDirectiveModel
    {
        [Description("Optional typename of the class the base class should inherit from")]
        [DataMember(Name = "type")]
        public string TypeName { get; set; }
    }
}
