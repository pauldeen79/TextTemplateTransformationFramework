using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class InjectedTemplateToken<TState> : TemplateToken<TState>, IRenderToken<TState>
        where TState : class
    {
        public InjectedTemplateToken(SectionContext<TState> context, string overrideFileName = null) : base(context, overrideFileName)
        {
        }

        public string Contents => "Hello world!";
    }
}
