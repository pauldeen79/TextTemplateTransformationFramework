using System;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TemplateCodeCompiler<TState> : ITemplateCodeCompiler<TState>
        where TState : class
    {
        private readonly ITemplateCodeCompiler<TState> _baseCompiler;
        private const string Key = "TextTemplateTransformationFramework.T4.Plus.TemplateCodeCompiler";

        public TemplateCodeCompiler(ITemplateCodeCompiler<TState> baseCompiler)
        {
            _baseCompiler = baseCompiler ?? throw new ArgumentNullException(nameof(baseCompiler));
        }

        public TemplateCompilerOutput<TState> Compile(ITextTemplateProcessorContext<TState> context,
                                                      TemplateCodeOutput<TState> codeOutput)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (codeOutput == null)
            {
                throw new ArgumentNullException(nameof(codeOutput));
            }

            if (context.ContainsKey(Key))
            {
                //escape recursion
                return _baseCompiler.Compile(context, codeOutput);
            }
            context.Add(Key, this);
            try
            {
                var callback = new Callback<TState, ITemplateCodeCompiler<TState>>(context, this);
                foreach (var interceptorToken in codeOutput.SourceTokens
                    .GetTemplateTokensFromSections<TState, ITemplateCompilerInterceptorToken<TState>>()
                    .Reverse()
                    .ToArray())
                {
                    var result = interceptorToken.Process(codeOutput, callback);
                    if (result != null)
                    {
                        return result;
                    }
                }
                return _baseCompiler.Compile(context, codeOutput);
            }
            finally
            {
                context.Remove(Key);
            }
        }
    }
}
