using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class Callback<TState, TCallback> : ICallback<TState, TCallback>
        where TState : class
        where TCallback : class
    {

        public ITextTemplateProcessorContext<TState> Context { get; }

        public TCallback Instance { get; }

        public Callback(ITextTemplateProcessorContext<TState> context, TCallback instance)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }
    }
}
