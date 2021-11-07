using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    internal class PropertyDescriptorTemplate : Runtime.T4PlusGeneratedTemplateBase
    {
        public object Model { get; set; }

        public void Render(StringBuilder builder)
        {
            GenerationEnvironment = builder;
            var property = TypeDescriptor.GetProperties(Model).Find("Name", false);
            Write(ToStringHelper.ToStringWithCulture(property?.GetValue(Model)));
            Write("!");
        }
    }
}
