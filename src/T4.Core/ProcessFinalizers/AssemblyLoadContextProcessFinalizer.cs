using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Core.ProcessFinalizers
{
    public class AssemblyLoadContextProcessFinalizer<TState> : IProcessFinalizer<ITextTemplateProcessorContext<TState>>
        where TState : class
    {
        public void Finalize(ITextTemplateProcessorContext<TState> context)
        {
            if (context?["CoreAssemblyLoadContext"] is AssemblyLoadContext loadContext)
            {
                loadContext.Unload();
            }
        }
    }
}
