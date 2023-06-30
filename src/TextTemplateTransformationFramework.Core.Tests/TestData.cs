namespace TextTemplateTransformationFramework.Core.Tests;

internal static class TestData
{
#if Windows
    internal const string BasePath = @"C:\Somewhere";
#elif Linux
    internal const string BasePath = @"/usr/bin/python3";
#elif OSX
    internal const string BasePath = @"/Users/moi/Downloads";
#else
    internal const string BasePath = "Unknown basepath, only Windows, Linux and OSX are supported";
#endif

    public class PlainTemplate
    {
        private readonly Func<string> _delegate;

        public PlainTemplate(Func<string> @delegate) => _delegate = @delegate;

        public override string ToString() => _delegate();
    }

    public class Template : ITemplate
    {
        private readonly Action<StringBuilder> _delegate;

        public Template(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    public class TemplateWithModel : ITemplate, IModelContainer
    {
        public IModel Model { get; set; } = default!;

        private readonly Action<StringBuilder> _delegate;

        public TemplateWithModel(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    public class TemplateWithViewModel : ITemplate, IViewModelContainer
    {
        public IModel ViewModel { get; set; } = default!;

        private readonly Action<StringBuilder> _delegate;

        public TemplateWithViewModel(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    public class PlainTemplateWithAdditionalParameters
    {
        public string AdditionalParameter { get; set; } = "";

        public override string ToString() => AdditionalParameter;
    }

    public class PlainTemplateWithModelAndAdditionalParameters : IModelContainer
    {
        public IModel Model { get; set; } = default!;

        public string AdditionalParameter { get; set; } = "";

        public override string ToString() => AdditionalParameter;
    }
}
