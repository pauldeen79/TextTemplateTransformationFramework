using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class CustomBaseClass
    {
        protected void Write(object value)
        {
            GenerationEnvironment.Append(ToStringHelper.ToStringWithCulture(value));
        }

        private Runtime.T4GeneratedTemplateBase.ToStringInstanceHelper _helper;

        protected Runtime.T4GeneratedTemplateBase.ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return _helper ?? (_helper = new Runtime.T4GeneratedTemplateBase.ToStringInstanceHelper());
            }
            set
            {
                _helper = value;
            }
        }

        private StringBuilder _generationEnvironment;

        protected StringBuilder GenerationEnvironment
        {
            get
            {
                return _generationEnvironment ?? (_generationEnvironment = new StringBuilder());
            }
            set
            {
                _generationEnvironment = value;
            }
        }
    }
}
