using System;
using System.IO;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Core.ProcessInitializers
{
    public class AssemblyLoadContextProcessInitializer<TState> : IProcessInitializer<ITextTemplateProcessorContext<TState>>
        where TState : class
    {
        public void Initialize(ITextTemplateProcessorContext<TState> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var loadContext = new CustomAssemblyLoadContext("Generation context", true, () => new[] { (context.TryGetValue("TempPath", out var value) ? value?.ToString()
                : null) ?? Path.GetTempPath() } );
            context["CoreAssemblyLoadContext"] = loadContext;
        }
    }
}
