using System;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors
{
    public interface IModeledTemplateSectionProcessor<TState> : INonDiscoverableTemplateSectionProcessor<TState>
        where TState : class
    {
        Type ModelType { get; }
    }
}
