﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace TextTemplateTransformationFramework.T4.Core
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public CustomAssemblyLoadContext(string name, bool isCollectible, Func<IEnumerable<string>> customPathsDelegate)
            : base(name, isCollectible)
        {
            CustomPathsDelegate = customPathsDelegate ?? throw new ArgumentNullException(nameof(customPathsDelegate));
        }

        private Func<IEnumerable<string>> CustomPathsDelegate { get; }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                return null;
            }

            if (assemblyName.Name == "netstandard")
            {
                return null;
            }

            var customPath = CustomPathsDelegate.Invoke()
                .Select(directory => Path.Combine(directory, assemblyName.Name + ".dll"))
                .FirstOrDefault(File.Exists);

            if (customPath == null)
            {
                return null;
            }

            return LoadFromAssemblyPath(customPath);
        }
    }
}
