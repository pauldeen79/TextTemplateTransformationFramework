﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using Utilities;

namespace TextTemplateTransformationFramework.T4.Plus.Cmd
{
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
            catch (FileLoadException fle) when (fle.Message.StartsWith("The given assembly name was invalid."))
            {
                if (assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) && !Path.IsPathFullyQualified(assemblyName))
                {
                    assemblyName = Path.Combine(Directory.GetCurrentDirectory(), assemblyName);
                }
                return context.LoadFromAssemblyPath(assemblyName);
            }
        }
    }
}