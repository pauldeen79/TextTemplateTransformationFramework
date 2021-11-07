using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class AdditionalChildTemplateDirective : IModelTypeCreator
    {
        public Type CreateType(Type genericType)
        {
            return typeof(AdditionalChildTemplateDirective<>).MakeGenericType(genericType);
        }
    }

    [ExcludeFromCodeCoverage]
    public class AdditionalChildTemplateDirective<TState> : IInitializableTemplateSectionProcessor<TState>
        where TState : class
    {
        public SectionProcessResult<TState> Initialize(SectionContext<TState> context)
            => Process(context);

        public SectionProcessResult<TState> Process(SectionContext<TState> context)
            => SectionProcessResult.Create(new ITemplateToken<TState>[]
            {
                new AdditionalChildTemplateToken<TState>(context, "RenderTokens.Injected", () => new InjectedTemplate<TState>(), typeof(InjectedTemplateToken<TState>)),
                new InjectedTemplateToken<TState>(context)
            });
    }
}
