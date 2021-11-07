using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class CustomSectionProcessorDirective : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
        {
            return typeof(CustomSectionProcessorDirective<>).MakeGenericType(genericType);
        }
    }

    [DirectivePrefix("myCustomSection")]
    [ExcludeFromCodeCoverage]
    public class CustomSectionProcessorDirective<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
        {
            return SectionProcessResult.Create(new RenderTextToken<TState>(context, "Hello world!"));
        }
    }
}
