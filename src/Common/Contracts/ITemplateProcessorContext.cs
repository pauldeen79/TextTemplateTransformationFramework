using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateProcessorContext<TState> : IDictionary<string, object>
        where TState : class
    {
        ITextTemplateProcessorContext<TState> TextTemplateProcessorContext { get; }
        TemplateCompilerOutput<TState> TemplateCompilerOutput { get; }
    }
}
