using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateSectionProcessorContainer<TState>
        where TState : class
    {
        IEnumerable<ITemplateSectionProcessor<TState>> ContainedTemplateSectionProcessors { get; }
    }
}
