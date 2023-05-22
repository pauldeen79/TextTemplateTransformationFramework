using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITextTemplateProcessorContext<TState> : IDictionary<string, object>
        where TState : class
    {
        TextTemplate TextTemplate { get; }
        AssemblyTemplate AssemblyTemplate { get; }
        TemplateParameter[] Parameters { get; }
        ILogger Logger { get; }
        SectionContext<TState> ParentContext { get; }
    }
}
