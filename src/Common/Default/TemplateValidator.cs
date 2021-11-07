using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateValidator : ITemplateValidator
    {
        public ProcessResult Validate(object template)
            => ProcessResult.Create(Array.Empty<CompilerError>());
    }
}
