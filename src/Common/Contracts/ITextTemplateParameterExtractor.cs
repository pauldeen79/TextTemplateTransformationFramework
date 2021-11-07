using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITextTemplateParameterExtractor<TState>
        where TState : class
    {
        IEnumerable<TemplateParameter> Extract(ITextTemplateProcessorContext<TState> context, TemplateCompilerOutput<TState> templateCompilerOutput);
    }
}
