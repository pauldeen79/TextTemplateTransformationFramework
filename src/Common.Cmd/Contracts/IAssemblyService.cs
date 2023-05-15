using System.Reflection;

namespace TextTemplateTransformationFramework.Common.Cmd.Contracts
{
    public interface IAssemblyService
    {
        public Assembly LoadAssembly(string assemblyName);

        public void SetCustomPath(string path);
    }
}
