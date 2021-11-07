using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class MessageTokenExtensions
    {
        public static CompilerError ToCompilerError<TState>(this IMessageToken<TState> instance)
            where TState : class
            => new CompilerError(1,
                                 "TemplateError",
                                 instance.Message,
                                 instance.FileName,
                                 instance is IWarningToken<TState>,
                                 instance.LineNumber);
    }
}
