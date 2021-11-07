using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class InterceptTokenProcessorDirective : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
        {
            return typeof(InterceptTokenProcessorDirective<>).MakeGenericType(genericType);
        }

        public static bool Processed { get; set; }
    }

    [DirectivePrefix("myCustomSection")]
    [ExcludeFromCodeCoverage]
    public class InterceptTokenProcessorDirective<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
        {
            return SectionProcessResult.Create(new TokenProcessorInterceptorToken<TState>(context, (tokens, callback) =>
            {
                InterceptTokenProcessorDirective.Processed = true;
                return callback.Instance.Process(callback.Context, tokens);
            }));
        }
    }
}
