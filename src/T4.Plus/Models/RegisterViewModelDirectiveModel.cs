using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class RegisterViewModelDirectiveModel : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
            => typeof(RegisterViewModelDirectiveModel<>).MakeGenericType(genericType);
    }

    [Description("Registers a child template")]
    public class RegisterViewModelDirectiveModel<TState> : RegisterChildTemplateDirectiveModel<TState>
        where TState : class
    {
    }
}
