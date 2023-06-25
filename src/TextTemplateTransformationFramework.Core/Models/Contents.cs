using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.Core.Models
{
    [DataContract(Name = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework")]
    public class Contents
    {
        [DataMember]
        public string FileName { get; set; } = "";

        [DataMember]
        public List<string> Lines { get; set; } = new();

        [DataMember]
        public bool SkipWhenFileExists { get; set; }
    }
}
