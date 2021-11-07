using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class RegisterChildTemplateDirectiveModel : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
            => typeof(RegisterChildTemplateDirectiveModel<>).MakeGenericType(genericType);
    }

    [Description("Registers a child template")]
    public class RegisterChildTemplateDirectiveModel<TState> : RegisterChildTemplateDirectiveModelSingleFileBase<TState>
        where TState : class
    {
        [Required]
        [Description("Filename of the child template")]
        public string FileName { get; set; }
    }
}
