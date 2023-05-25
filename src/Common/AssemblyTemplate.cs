using System;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common
{
    public class AssemblyTemplate : ITemplate
    {
        public string AssemblyName { get; }
        public string ClassName { get; }
        public AssemblyLoadContext AssemblyLoadContext { get; }

        public AssemblyTemplate(string assemblyName, string className, AssemblyLoadContext assemblyLoadContext)
        {
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
            AssemblyLoadContext = assemblyLoadContext ?? throw new ArgumentNullException(nameof(assemblyLoadContext));
        }
    }
}
