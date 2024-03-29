﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.Runtime
{
    [DataContract(Name = "MultipleContents", Namespace = "http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework")]
    public class MultipleContents
    {
        [DataMember]
        public string BasePath { get; set; }

        [DataMember]
        public List<Contents> Contents { get; set; }

        public const string XmlStringFragment = @"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformation"">";
    }
}