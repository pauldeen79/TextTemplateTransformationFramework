using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.Interception
{
    public class TemplateCompilerInterceptorToken<TState> : TemplateToken<TState>, ITemplateCompilerInterceptorToken<TState>
        where TState : class
    {
        private readonly Func<TemplateCodeOutput<TState>, ICallback<TState, ITemplateCodeCompiler<TState>>, TemplateCompilerOutput<TState>> _processDelegate;

        public TemplateCompilerInterceptorToken(SectionContext<TState> context, Func<TemplateCodeOutput<TState>, ICallback<TState, ITemplateCodeCompiler<TState>>, TemplateCompilerOutput<TState>> processDelegate)
            : base(context)
            => _processDelegate = processDelegate ?? throw new ArgumentNullException(nameof(processDelegate));

        public TemplateCompilerOutput<TState> Process(TemplateCodeOutput<TState> codeOutput, ICallback<TState, ITemplateCodeCompiler<TState>> callback)
            => _processDelegate(codeOutput, callback);
    }
}
