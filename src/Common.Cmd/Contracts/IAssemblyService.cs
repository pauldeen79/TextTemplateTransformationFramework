using System.Reflection;
using System.Runtime.Loader;

namespace TextTemplateTransformationFramework.Common.Cmd.Contracts
{
    public interface IAssemblyService
    {
        public Assembly LoadAssembly(string assemblyName, AssemblyLoadContext context);

        public void SetCustomPath(string path);
    }
}
