using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ScriptCompiler.NetFramework;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.NetFramework
{
    /// <summary>
    /// Default implementation for ITemplateCodeCompiler, which uses CodeDom to compile code (currently, only C# or Vb.Net).
    /// </summary>
    /// <seealso cref="ITemplateCodeCompiler" />
    [Serializable]
    public class TemplateCodeCompiler<TState> : ITemplateCodeCompiler<TState>
        where TState : class
    {
        private readonly IScriptCompiler _scriptCompiler;

        public TemplateCodeCompiler(IScriptCompiler scriptCompiler)
        {
            _scriptCompiler = scriptCompiler ?? throw new ArgumentNullException(nameof(scriptCompiler));
        }

        public TemplateCompilerOutput<TState> Compile(ITextTemplateProcessorContext<TState> context,
                                                      TemplateCodeOutput<TState> codeOutput)
        {
            if (codeOutput == null)
            {
                throw new ArgumentNullException(nameof(codeOutput));
            }

            var referencedAssemblies = codeOutput.ReferencedAssemblies.ToList();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var name = assembly.Location.Split('\\').Last();
                if (name.StartsWith("System", StringComparison.OrdinalIgnoreCase)
                    && !referencedAssemblies.Any(x => x.Split('\\').Last().Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    referencedAssemblies.Add(assembly.Location);
                }
            }

            Assembly resolve(object sender, ResolveEventArgs args)
            {
                var name = args.Name.GetAssemblyName();
                foreach (var reference in referencedAssemblies)
                {
                    if (reference.EndsWith(name, StringComparison.OrdinalIgnoreCase))
                    {
#pragma warning disable S3885 // "Assembly.Load" should be used
                        return Assembly.LoadFrom(reference);
#pragma warning restore S3885 // "Assembly.Load" should be used
                    }
                }
                return null;
            }
            AppDomain.CurrentDomain.AssemblyResolve += resolve;
            try
            {
                var result = _scriptCompiler.LoadScriptToMemory
                (
                    codeOutput.SourceCode,
                    codeOutput.Language,
                    referencedAssemblies.DistinctBy(s => s.ToUpper(CultureInfo.InvariantCulture)),
                    codeOutput.PackageReferences.DistinctBy(x => x.ToUpper(CultureInfo.InvariantCulture)),
                    codeOutput.TempPath,
                    null
                );
                var template = result.Errors.HasErrors
                    ? null
                    : CreateTemplate(codeOutput, result);

                if (result.Errors.HasErrors)
                {
                    //when compilation fails, add parse-time errors.
                    result.Errors.AddRange
                    (
                        codeOutput
                        .SourceTokens
                        .GetTemplateTokensFromSections<TState, IMessageToken<TState>>()
                        .Select(e => e.ToCompilerError())
                        .Select(e => new System.CodeDom.Compiler.CompilerError(e.FileName, e.Line, e.Column, e.ErrorNumber, e.ErrorText))
                        .ToArray()
                    );
                }

                return TemplateCompilerOutput.Create
                (
                    GetCompiledAssembly(result),
                    template,
                    result.Errors.OfType<System.CodeDom.Compiler.CompilerError>().Select(e => new CompilerError(e.Column, e.ErrorNumber, e.ErrorText, e.FileName, e.IsWarning, e.Line)),
                    codeOutput.SourceCode,
                    codeOutput.OutputExtension,
                    codeOutput.SourceTokens,
                    codeOutput.Errors
                );
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= resolve;
            }
        }

        private static Assembly GetCompiledAssembly(System.CodeDom.Compiler.CompilerResults result)
        {
            try
            {
                return result.CompiledAssembly;
            }
            catch (Exception ex)
            {
                result.Errors.Add(new System.CodeDom.Compiler.CompilerError("unknown", 0, 0, "unknown", ex.ToString()));
                return null;
            }
        }

        protected virtual object CreateTemplate(TemplateCodeOutput<TState> codeOutput, System.CodeDom.Compiler.CompilerResults result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (codeOutput == null)
            {
                throw new ArgumentNullException(nameof(codeOutput));
            }

            return result.CompiledAssembly.CreateInstance(codeOutput.ClassName);
        }
    }
}
