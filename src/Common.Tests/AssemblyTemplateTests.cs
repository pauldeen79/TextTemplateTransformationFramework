using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    public class AssemblyTemplateTests : TestBase
    {
        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(AssemblyTemplate));
        }
    }
}
