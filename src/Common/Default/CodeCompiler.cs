using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ScriptCompiler;
using TextTemplateTransformationFramework.Common.Contracts;
#if NETFRAMEWORK
using Utilities.Extensions;
#endif

namespace TextTemplateTransformationFramework.Common.Default
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
            if (codeOutput == null)
            {
                throw new ArgumentNullException(nameof(codeOutput));
            }

            var referencedAssemblies = codeOutput.ReferencedAssemblies.ToList();
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
                referencedAssemblies.DistinctBy(x => x.ToUpper(CultureInfo.InvariantCulture)),
                packageReferences.DistinctBy(x => x.ToUpper(CultureInfo.InvariantCulture)),
                codeOutput.TempPath,
                null,
                null
            );
        }
    }
}
