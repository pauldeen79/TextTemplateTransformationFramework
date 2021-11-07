using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Core.ProcessInitializers
{
    public class PlusCoreProcessInitializer<TState> : IProcessInitializer<ITextTemplateProcessorContext<TState>>
        where TState : class
    {
        public void Initialize(ITextTemplateProcessorContext<TState> context)
        {
            new Plus.ProcessInitializers.TraceTextTemplateProcessorInitializer<TState>().Initialize(context);
            new T4.Core.ProcessInitializers.AssemblyLoadContextProcessInitializer<TState>().Initialize(context);
        }
    }
}
