using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception
{
    public interface ITemplateCompilerInterceptorToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        TemplateCompilerOutput<TState> Process(TemplateCodeOutput<TState> codeOutput, ICallback<TState, ITemplateCodeCompiler<TState>> callback);
    }
}
