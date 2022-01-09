using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    [Description("Renders a child template")]
    public class RenderChildTemplateDirectiveModel : IValidatableObject
    {
        [Description("Child template name")]
        public string Name { get; set; }

        [Description("Indicator whether the child template name is a literal expression; true by default")]
        [DefaultValue(true)]
        public bool NameIsLiteral { get; set; }

        [Description("Optional model expression or literal")]
        public string Model { get; set; }

        [Description("Indicator whether the model is a literal expression; false by default")]
        public bool ModelIsLiteral { get; set; }

        [Description("Indicator whether the child template needs to render the model as enumerable; null by default (which means auto detect)")]
        public bool? Enumerable { get; set; }

        [Description("Indicator whether to continue on errors; false by default")]
        public bool SilentlyContinueOnError { get; set; }

        [Description("Separator template name; only used for enumerables")]
        public string SeparatorTemplateName { get; set; }

        [Description("Indicator whether the separator template name is a literal expression; true by default")]
        [DefaultValue(true)]
        public bool SeparatorTemplateNameIsLiteral { get; set; }

        [Description("Custom resolver delegate expression or literal (optional)")]
        public string CustomResolverDelegate { get; set; }

        [Description("Indicator whether the custom resolver delegate is a literal expression; false by default")]
        public bool CustomResolverDelegateIsLiteral { get; set; }

        [Description("Custom render child template delegate expression or literal (optional)")]
        public string CustomRenderChildTemplateDelegate { get; set; }

        [Description("Indicator whether the custom render child template delegate is a literal expression; false by default")]
        public bool CustomRenderChildTemplateDelegateIsLiteral { get; set; }

        [Description("Optional model for the resolver delegate; when empty, the model will be used")]
        public string ResolverDelegateModel { get; set; }

        [Description("Indicator whether the resolver delegate model is a literal expression; false by default")]
        public bool ResolverDelegateModelIsLiteral { get; set; }

        [Description("Custom template name delegate expression or literal (optional)")]
        public string CustomTemplateNameDelegate { get; set; }

        [Description("Indicator whether the custom template name delegate is a literal expression; false by default")]
        public bool CustomTemplateNameDelegateIsLiteral { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Model))
            {
                yield return new ValidationResult("Property name or model is required on renderChildTemplate directive", new[] { nameof(Name), nameof(Model) });
            }
        }
    }
}
