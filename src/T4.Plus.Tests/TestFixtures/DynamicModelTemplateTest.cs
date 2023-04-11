using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    internal sealed class DynamicModelTemplateTest : Runtime.T4PlusGeneratedTemplateBase
    {
        public dynamic Model { get; set; }

        public void Render(StringBuilder builder)
        {
            GenerationEnvironment = builder;
            Write(ToStringHelper.ToStringWithCulture(Model.Name));
            Write("!");
        }
    }
}
