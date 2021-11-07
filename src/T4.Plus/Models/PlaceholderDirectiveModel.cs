using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class PlaceholderDirectiveModel : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
            => typeof(PlaceholderDirectiveModel<>).MakeGenericType(genericType);
    }

    [Description("Registers and renders placeholder")]
    public class PlaceholderDirectiveModel<TState> : RegisterChildTemplateDirectiveModelSingleFileBase<TState>
        where TState : class
    {
        [Description("Optional model expression or literal")]
        public string Model { get; set; }

        [Description("Indicator whether the model is a literal expression; false by default")]
        public bool ModelIsLiteral { get; set; }

        [Description("Indicator whether the child template needs to render the model as enumerable; null by default (which means auto detect)")]
        public bool? Enumerable { get; set; }

        [Description("Indicator whether to continue on errors; false by default")]
        public bool SilentlyContinueOnError { get; set; }
    }
}
