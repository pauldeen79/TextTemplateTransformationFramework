using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateInitializer<TState> : ITemplateInitializer<TState>
        where TState : class
    {
        private readonly ITemplateInitializeParameterSetter<TState> _templateInitializeParameterSetter;
        private readonly ITemplateProxy _templateProxy;

        public TemplateInitializer(ITemplateInitializeParameterSetter<TState> templateInitializeParameterSetter,
                                   ITemplateProxy templateProxy)
        {
            _templateInitializeParameterSetter = templateInitializeParameterSetter ?? throw new ArgumentNullException(nameof(templateInitializeParameterSetter));
            _templateProxy = templateProxy ?? throw new ArgumentNullException(nameof(templateProxy));
        }

        public void Initialize(ITemplateProcessorContext<TState> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            SetParametersBeforeInitialize(context);

            _templateProxy.Initialize(context.TemplateCompilerOutput.Template);
        }

        private void SetParametersBeforeInitialize(ITemplateProcessorContext<TState> context)
            => _templateInitializeParameterSetter.Set(context);
    }
}
