using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.T4.Core;
using Utilities;

namespace TextTemplateTransformationFramework.T4.Plus.Cmd
{
    public class AssemblyService : IAssemblyService
    {
        private string _customPath = Directory.GetCurrentDirectory();

        public Assembly LoadAssembly(string assemblyName, AssemblyLoadContext context)
        {
            Guard.AgainstNull(context, nameof(context));

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
