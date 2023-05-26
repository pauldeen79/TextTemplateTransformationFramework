using CrossCutting.Common.Testing;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    public class AssemblyTemplateTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(AssemblyTemplate));
        }
    }
}
