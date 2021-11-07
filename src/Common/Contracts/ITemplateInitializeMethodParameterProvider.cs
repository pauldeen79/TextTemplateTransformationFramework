using System.Reflection;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateInitializeMethodParameterProvider
    {
        object[] Get(MethodInfo initializeMethod, object template);
    }
}
