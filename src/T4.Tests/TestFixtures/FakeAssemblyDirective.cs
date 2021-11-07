using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Models;

namespace TextTemplateTransformationFramework.T4.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class FakeAssemblyDirective : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
        {
            return typeof(FakeAssemblyDirective<>).MakeGenericType(genericType);
        }
    }

    [DirectivePrefix("assembly")]
    [DirectiveModel(typeof(AssemblyDirectiveModel))]
    [ExcludeFromCodeCoverage]
    public class FakeAssemblyDirective<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => throw new NotImplementedException();
    }
}
