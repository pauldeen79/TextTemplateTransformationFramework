using System;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateInitializeMethodParameterProvider : ITemplateInitializeMethodParameterProvider
    {
        public object[] Get(MethodInfo initializeMethod, object template)
        {
            if (initializeMethod is null)
            {
                throw new ArgumentNullException(nameof(initializeMethod));
            }

            return initializeMethod.GetParameters().Length == 0
                ? Array.Empty<object>()
                : new object[] { null };
        }
    }
}
