using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public abstract class RegisterChildTemplateDirectiveModelSingleFileBase<TState> : RegisterChildTemplateDirectiveModelBase<TState>, IValidatableObject
        where TState : class
    {
        [Description("Name of the child template")]
        public string Name { get; set; }

        [Description("Indicator whether the child template name is a literal expression; true by default")]
        [DefaultValue(true)]
        public bool NameIsLiteral { get; set; }

        [Description("Optional typename of the model")]
        [DataMember(Name = "modelType")]
        public string ModelTypeName { get; set; }

        [Description("When set to true, do not add a Model property to the template, but only use the model type for routing purposes")]
        public bool UseModelForRoutingOnly { get; set; }

        [Description("When set to false, do not add the model type to the routing configuration. In this case, the template can only be called directly.")]
        [DefaultValue(true)]
        public bool UseModelForRouting { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("Name is required", new[] { nameof(Name) });
            }
        }
    }
}
