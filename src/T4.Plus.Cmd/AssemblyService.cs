using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using Utilities;

namespace TextTemplateTransformationFramework.T4.Plus.Cmd
{
#if Windows
#else
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1062 // Validate arguments of public methods, false positive because we've handled it in the Guard.AgainstNull method above
    public class AssemblyService : IAssemblyService
    {
        public string[] GetCustomPaths(string assemblyName)
        {
            Guard.AgainstNull(assemblyName, nameof(assemblyName));

            if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathFullyQualified(assemblyName))
            {
                return new[] { Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), assemblyName)) };
            }

            return Array.Empty<string>();
        }

        public Assembly LoadAssembly(string assemblyName, AssemblyLoadContext context)
        {
            Guard.AgainstNull(assemblyName, nameof(assemblyName));
            Guard.AgainstNull(context, nameof(context));

            try
            {
                return context.LoadFromAssemblyName(new AssemblyName(assemblyName));
            }
            catch (Exception e) when (e.Message.StartsWith("The given assembly name was invalid.") || e.Message.EndsWith("The system cannot find the file specified."))
            {
                if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathFullyQualified(assemblyName))
                {
                    assemblyName = Path.Combine(Directory.GetCurrentDirectory(), assemblyName);
                }
                return context.LoadFromAssemblyPath(assemblyName);
            }
        }
    }
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore IDE0079 // Remove unnecessary suppression
}
