using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.ProcessInitializers
{
    internal static class TemplateProcessorInitializer
    {
        internal static readonly object _lock = new object();
    }

    public class TemplateProcessorInitializer<TState> : IProcessInitializer<ITemplateProcessorContext<TState>>
        where TState : class
    {

        public void Initialize(ITemplateProcessorContext<TState> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            
            Func<AssemblyLoadContext, AssemblyName, Assembly> handler = (sender, args) => CustomResolving(sender, args, context);
            AssemblyLoadContext.Default.Resolving += handler;
            context["TemplateProcessorDecorator.SourceTokens"] = context.TemplateCompilerOutput.SourceTokens;
            context["TemplateProcessorDecorator.Handler"] = handler;
        }

        private Assembly CustomResolving(AssemblyLoadContext sender, AssemblyName args, ITemplateProcessorContext<TState> state)
        {
            lock (TemplateProcessorInitializer._lock)
            {
                if (!(state["TemplateProcessorDecorator.SourceTokens"] is IEnumerable<ITemplateToken<TState>> sourceTokens))
                {
                    return null;
                }

                var tempPath = sourceTokens
                    .GetTemplateTokensFromSections<TState, ITempPathToken<TState>>()
                    .LastOrDefault()?.Value; //design decision: when multiple values are found, use the last one

                var hintPathTokens = sourceTokens
                    .GetTemplateTokensFromSections<TState, IHintPathToken<TState>>()
                    .Where(t => t.Name?.Equals(args.Name, StringComparison.OrdinalIgnoreCase) != false);

                var assemblyPath = hintPathTokens
                    .SelectMany(hintPathToken => hintPathToken.HintPath.GetDirectories(hintPathToken.Recursive))
                    .Select(directory => Path.Combine(directory, args.Name + ".dll"))
                    .Concat(new[] { (Path.Combine(tempPath ?? Path.GetTempPath(), args.Name + ".dll")) })
                    .FirstOrDefault(File.Exists);

                return assemblyPath != null
                    ? sender.LoadFromAssemblyPath(assemblyPath)
                    : null;
            }
        }
    }
}
