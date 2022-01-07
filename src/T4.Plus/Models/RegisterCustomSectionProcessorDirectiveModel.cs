using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class RegisterCustomSectionProcessorDirectiveModel : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
            => typeof(RegisterCustomSectionProcessorDirectiveModel<>).MakeGenericType(genericType);
    }

    [Description("Registers a custom section processor directive")]
    public class RegisterCustomSectionProcessorDirectiveModel<TState> : ISectionContextContainer<TState>
        where TState : class
    {
        [Description("Name of the assembly that contains the custom section processor")]
        public string AssemblyName { get; set; }

        [Description("Typename of the custom section processor, or empty to use all section processors from the specified assembly. Required when file is filled.")]
        public string TypeName { get; set; }

        [Description("Directive prefix in case of source code")]
        public string Prefix { get; set; }

        [Browsable(false)]
        public SectionContext<TState> SectionContext { get; set; }
    }
}
