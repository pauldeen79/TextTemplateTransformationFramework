using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class AssemblyDirectiveModel : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
            => typeof(AssemblyDirectiveModel<>).MakeGenericType(genericType);
    }

    [Description("Adds an assembly reference or hint path")]
    public class AssemblyDirectiveModel<TState> : IValidatableObject, ISectionContextContainer<TState>
        where TState : class
    {
        [Description("Assembly name")]
        public string Name { get; set; }

        [Description("Hint path to get the assembly")]
        public string HintPath { get; set; }

        [Description("Indicator whether the hint path should be searched recursively")]
        public bool Recursive { get; set; }

        [Description("Indicator whether the assembly needs to be pre-loaded, so custom section processor types can be used during the parse process")]
        public bool PreLoad { get; set; }

        [Description("Optional framework name (for example .NETCoreApp or .NETFramework) that the current entry assembly should start with")]
        public string FrameworkFilter { get; set; }

        [Browsable(false)]
        public SectionContext<TState> SectionContext { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(HintPath))
            {
                yield return new ValidationResult("Either Name or HintPath is required", new[] { nameof(Name), nameof(HintPath) });
            }
        }
    }
}
