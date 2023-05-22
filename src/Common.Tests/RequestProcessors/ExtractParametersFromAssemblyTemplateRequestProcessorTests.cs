using CrossCutting.Common.Testing;
using TextTemplateTransformationFramework.Common.RequestProcessors;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.RequestProcessors
{
    public class ExtractParametersFromAssemblyTemplateRequestProcessorTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ExtractParametersFromAssemblyTemplateRequestProcessor<ExtractParametersFromAssemblyTemplateRequestProcessorTests>));
        }
    }
}
