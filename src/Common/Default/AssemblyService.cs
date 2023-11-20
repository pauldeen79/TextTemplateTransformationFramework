using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities;

namespace TextTemplateTransformationFramework.Common.Default
{
#if Windows
#else
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    public class AssemblyService : IAssemblyService
    {
        public string[] GetCustomPaths(string assemblyName)
        {
            if (assemblyName is null) throw new ArgumentNullException(nameof(assemblyName));

            if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathRooted(assemblyName))
            {
                return new[] { Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), assemblyName)) };
            }

            return Array.Empty<string>();
        }

        public Assembly LoadAssembly(string assemblyName, AssemblyLoadContext context)
        {
            if (assemblyName is null) throw new ArgumentNullException(nameof(assemblyName));
            if (context is null) throw new ArgumentNullException(nameof(context));

            try
            {
                return context.LoadFromAssemblyName(new AssemblyName(assemblyName));
            }
            catch (Exception e) when (e.Message.StartsWith("The given assembly name was invalid.") || e.Message.EndsWith("The system cannot find the file specified."))
            {
                if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathRooted(assemblyName))
                {
                    assemblyName = Path.Combine(Directory.GetCurrentDirectory(), assemblyName);
                }
                return context.LoadFromAssemblyPath(assemblyName);
            }
        }
    }
}
