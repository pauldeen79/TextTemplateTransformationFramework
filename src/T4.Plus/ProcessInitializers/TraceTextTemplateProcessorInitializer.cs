using System.Diagnostics;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.ProcessInitializers
{
    public class TraceTextTemplateProcessorInitializer<TState> : IProcessInitializer<ITextTemplateProcessorContext<TState>>
        where TState : class
    {
        private bool _initialized;

        public void Initialize(ITextTemplateProcessorContext<TState> context)
        {
            if (_initialized)
            {
                return;
            }

            var delegateTraceListener = new DelegateTraceListener(message => context.Logger.Log(message));
            Trace.Listeners.Add(delegateTraceListener);

            _initialized = true;
        }
    }
}
