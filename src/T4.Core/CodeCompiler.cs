using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using ScriptCompiler;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Core
{
    public class CodeCompiler<TState> : ICodeCompiler<TState>
        where TState : class
        {
        private readonly IScriptCompiler _scriptCompiler;

        public CodeCompiler(IScriptCompiler scriptCompiler)
        {
            _scriptCompiler = scriptCompiler ?? throw new ArgumentNullException(nameof(scriptCompiler));
        }

        public CompilerResults Compile(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput)
        {
            if ((context?.ContainsKey("CoreAssemblyLoadContext")) != true
                || !(context?["CoreAssemblyLoadContext"] is AssemblyLoadContext loadContext)
                || codeOutput == null)
            {
                throw new InvalidOperationException("Can't find AssemblyLoadContext. Did you register the TextTemplateProcessorInitializer?");
            }
            var referencedAssemblies = new List<string>(codeOutput.ReferencedAssemblies ?? Enumerable.Empty<string>());
            var packageReferences = new List<string>(codeOutput.PackageReferences ?? Enumerable.Empty<string>());
            if (!packageReferences.Any())
            {
                packageReferences.Add("NETStandard.Library,2.0.3,.NETStandard,Version=v2.0");
                packageReferences.Add("System.ComponentModel.Annotations,5.0.0,.NETStandard,Version=v2.0");
                var netStandardAssemblyPath = AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic && !string.IsNullOrEmpty(asm.Location) && asm.Location.Contains("netstandard")).Select(x => x.Location).First();
                referencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(netStandardAssemblyPath), "System.ComponentModel.dll"));
            }

            return _scriptCompiler.LoadScriptToMemory
            (
                codeOutput.SourceCode,
                referencedAssemblies.DistinctBy(s => s.ToUpper(CultureInfo.InvariantCulture)),
                packageReferences.DistinctBy(x => x.ToUpper(CultureInfo.InvariantCulture)),
                codeOutput.TempPath,
                null,
                loadContext
            );
        }
    }
}
