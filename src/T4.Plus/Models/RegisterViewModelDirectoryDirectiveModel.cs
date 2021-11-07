using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class RegisterViewModelDirectoryDirectiveModel : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
            => typeof(RegisterViewModelDirectoryDirectiveModel<>).MakeGenericType(genericType);
    }

    [Description("Registers a view model directory")]
    public class RegisterViewModelDirectoryDirectiveModel<TState> : RegisterChildTemplateDirectoryDirectiveModel<TState>
        where TState : class
    {
    }
}
