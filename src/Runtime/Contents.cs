using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.Runtime
{
    [DataContract(Name = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework")]
    public class Contents
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public List<string> Lines { get; set; }

        [DataMember]
        public bool SkipWhenFileExists { get; set; }
    }
}