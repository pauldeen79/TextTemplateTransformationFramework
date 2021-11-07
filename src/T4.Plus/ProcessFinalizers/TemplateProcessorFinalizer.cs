using System;
using System.Reflection;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.ProcessFinalizers
{
    public class TemplateProcessorFinalizer<TState> : IProcessFinalizer<ITemplateProcessorContext<TState>>
        where TState : class
    {
        public void Finalize(ITemplateProcessorContext<TState> context)
        {
            var handler = context?["TemplateProcessorDecorator.Handler"] as Func<AssemblyLoadContext, AssemblyName, Assembly>;
            if (handler != null)
            {
                AssemblyLoadContext.Default.Resolving -= handler;
            }
        }
    }
}
