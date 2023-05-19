using System.Reflection;
using System.Runtime.Loader;

namespace TextTemplateTransformationFramework.Common.Cmd.Contracts
{
    public interface IAssemblyService
    {
        string[] GetCustomPaths(string assemblyName);
        public Assembly LoadAssembly(string assemblyName, AssemblyLoadContext context);
    }
}
