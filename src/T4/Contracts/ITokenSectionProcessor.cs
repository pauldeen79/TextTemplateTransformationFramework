using System;
using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenSectionProcessor<TState>
        where TState : class
    {
        ProcessSectionResult<TState> Process(SectionContext<TState> context, Type sectionProcessorType, SectionProcessResult<TState> sectionProcessResult);
    }
}
