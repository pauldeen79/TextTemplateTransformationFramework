using System.IO;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.T4.Core;

namespace TextTemplateTransformationFramework.T4.Plus.Cmd
{
    public class AssemblyService : IAssemblyService
    {
        private string _customPath = Directory.GetCurrentDirectory();

        public Assembly LoadAssembly(string assemblyName)
        {
            var context = new CustomAssemblyLoadContext("T4PlusCmd", true, () => new[] { _customPath });

            try
            {
                return context.LoadFromAssemblyName(new AssemblyName(assemblyName));
            }
            catch (FileLoadException fle) when (fle.Message.StartsWith("The given assembly name was invalid."))
            {
                return context.LoadFromAssemblyPath(assemblyName);
            }
        }

        public void SetCustomPath(string path) => _customPath = path;
    }
}
