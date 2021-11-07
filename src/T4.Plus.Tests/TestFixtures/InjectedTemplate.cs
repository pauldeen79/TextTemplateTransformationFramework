using System.Diagnostics.CodeAnalysis;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class InjectedTemplate<TState>
        where TState : class
    {
        public InjectedTemplateToken<TState> Model { get; set; }

        public override string ToString()
        {
            return "Write(\"" + Model.Contents + "\");";
        }
    }
}
