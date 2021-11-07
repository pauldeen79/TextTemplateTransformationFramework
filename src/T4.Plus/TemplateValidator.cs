using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TemplateValidator : ITemplateValidator
    {
        public ProcessResult Validate(object template)
            => ProcessResult.Create
                (
                    template.TryValidate(out ICollection<ValidationResult> validationResults)
                        ? Enumerable.Empty<CompilerError>()
                        : validationResults.Select
                        (
                            result =>
                            new CompilerError
                            (
                                0,
                                "Validation",
                                result.ErrorMessage,
                                template.GetType().Name + ".cs",
                                false,
                                0
                            )
                        )
                    .ToArray()
                );
    }
}
