using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    public class ViewModelTemplate
    {
        public string Property { get; set; }
        public MyViewModel ViewModel { get; set; }
        public IDictionary<string, object> Session { get; set; } = new Dictionary<string, object>();
        public object TemplateContext { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class MyViewModel
    {
        public string Property { get; set; }
        public string ViewModelProperty { get; set; }
        [DefaultValue("Default")]
        public string PropertyWithDefaultValue { get; set; }
        public object TemplateContext { get; set; }
    }
}
