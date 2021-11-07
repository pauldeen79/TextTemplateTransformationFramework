using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Models;

namespace TextTemplateTransformationFramework.Common.Tests
{
    [DirectivePrefix("import")]
    [DirectiveModel(typeof(ImportDirectiveModel))]
    [ExcludeFromCodeCoverage]
    public class FakeImportDirective<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => throw new NotImplementedException();
    }
}
