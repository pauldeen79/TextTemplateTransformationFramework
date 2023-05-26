using CrossCutting.Common.Testing;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Requests
{
    public class ExtractParametersFromTextTemplateRequestTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_Context()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ExtractParametersFromTextTemplateRequest<ExtractParametersFromTextTemplateRequestTests>));
        }
    }
}
