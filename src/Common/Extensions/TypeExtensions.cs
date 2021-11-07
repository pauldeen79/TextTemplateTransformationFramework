using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetModelType(this Type instance, Type genericType)
        {
            if (typeof(IModelTypeCreator).IsAssignableFrom(instance))
            {
                return ((IModelTypeCreator)Activator.CreateInstance(instance)).CreateType(genericType);
            }

            if (instance.IsGenericTypeDefinition)
            {
                var typeParams = instance.GetGenericArguments();
                if (typeParams.Length == 1 && typeParams[0].Name == "TState")
                {
                    return instance.MakeGenericType(genericType);
                }
            }

            return instance;
        }
    }
}
