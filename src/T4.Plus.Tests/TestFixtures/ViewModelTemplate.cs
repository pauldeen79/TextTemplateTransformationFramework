using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class ViewModelTemplate
    {
        public string Property { get; set; }
        public MyViewModel ViewModel { get; set; }
        public IDictionary<string, object> Session { get; set; } = new Dictionary<string, object>();
    }

    [ExcludeFromCodeCoverage]
    public class MyViewModel
    {
        public string Property { get; set; }
    }
}
