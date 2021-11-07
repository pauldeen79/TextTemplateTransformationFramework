using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class InjectedTokenParserInterceptorDirective : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
        {
            return typeof(InjectedTokenParserInterceptorDirective<>).MakeGenericType(genericType);
        }
    }

    [ExcludeFromCodeCoverage]
    public class InjectedTokenParserInterceptorDirective<TState> : IInitializableTemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Initialize(SectionContext<TState> context)
            => Process(context);

        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create(new ITemplateToken<TState>[]
            {
                new TokenParserInterceptorTokenMock<TState>(context)
            });
    }
}
