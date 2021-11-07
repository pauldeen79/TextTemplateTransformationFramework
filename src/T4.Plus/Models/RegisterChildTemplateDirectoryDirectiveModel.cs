using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class RegisterChildTemplateDirectoryDirectiveModel : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
            => typeof(RegisterChildTemplateDirectoryDirectiveModel<>).MakeGenericType(genericType);
    }

    [Description("Registers a child template directory")]
    public class RegisterChildTemplateDirectoryDirectiveModel<TState> : RegisterChildTemplateDirectiveModelBase<TState>, IValidatableObject
        where TState : class
    {
        [Description("Path of the child templates")]
        public string Path { get; set; }

        [Description("Search pattern")]
        public string SearchPattern { get; set; }

        [Description("Indicator to recursively search the directory; default is false")]
        public bool Recurse { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Path))
            {
                yield return new ValidationResult("Path is required", new[] { nameof(Path) });
            }
        }
    }
}
