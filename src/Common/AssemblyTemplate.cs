using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common
{
    public class AssemblyTemplate : ITemplate
    {
        public string AssemblyName { get; }
        public string ClassName { get; }

        public AssemblyTemplate(string assemblyName, string className)
        {
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
        }
    }
}
