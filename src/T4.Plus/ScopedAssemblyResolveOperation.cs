using System;
using System.Reflection;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public static class ScopedAssemblyResolveOperation
    {
        public static TReturn Invoke<TReturn>(Func<object, ResolveEventArgs, Assembly> assemblyResolveDelegate, Func<TReturn> function)
        {
            if (assemblyResolveDelegate is null)
            {
                throw new ArgumentNullException(nameof(assemblyResolveDelegate));
            }

            if (function is null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            var handler = new ResolveEventHandler(assemblyResolveDelegate);
            AppDomain.CurrentDomain.AssemblyResolve += handler;
            try
            {
                return function();
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= handler;
            }
        }
    }
}
