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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var loadContext = new CustomAssemblyLoadContext("Generation context", true, () => new[] { (context.ContainsKey("TempPath")
                ? context["TempPath"]?.ToString()
                : null) ?? Path.GetTempPath() } );
            context["CoreAssemblyLoadContext"] = loadContext;
        }
    }
}
