using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.Interception;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class InterceptTemplateCompilerDirective : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
        {
            return typeof(InterceptTemplateCompilerDirective<>).MakeGenericType(genericType);
        }

        public static bool Processed { get; set; }
    }

    [DirectivePrefix("myCustomSection")]
    [ExcludeFromCodeCoverage]
    public class InterceptTemplateCompilerDirective<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Process(SectionContext<TState> context)
        {
            return SectionProcessResult.Create(new TemplateCompilerInterceptorToken<TState>(context, (codeOutput, callback) =>
            {
                InterceptTemplateCompilerDirective.Processed = true;
                return callback.Instance.Compile(callback.Context, codeOutput);
            }));
        }
    }
}
