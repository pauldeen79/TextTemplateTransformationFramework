using System;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IModelTypeCreator
    {
        Type CreateType(Type genericType);
    }
}
