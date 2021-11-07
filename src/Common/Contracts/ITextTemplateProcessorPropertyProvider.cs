using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITextTemplateProcessorPropertyProvider<TState>
        where TState : class
    {
        IEnumerable<PropertyDescriptor> Get(ITextTemplateProcessorContext<TState> context,
                                            TemplateCompilerOutput<TState> templateCompilerOutput,
                                            Type templateType);
    }
}
