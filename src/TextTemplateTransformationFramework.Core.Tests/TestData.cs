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

    internal sealed class PlainTemplate
    {
        private readonly Func<string> _delegate;

        public PlainTemplate(Func<string> @delegate) => _delegate = @delegate;

        public override string ToString() => _delegate();
    }

    internal sealed class Template : ITemplate
    {
        private readonly Action<StringBuilder> _delegate;

        public Template(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    internal sealed class TemplateWithModel<T> : ITemplate, IModelContainer<T>
    {
        public T? Model { get; set; } = default!;

        private readonly Action<StringBuilder> _delegate;

        public TemplateWithModel(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    internal sealed class TemplateWithViewModel<T> : ITemplate, IViewModelContainer<T>
    {
        public T? ViewModel { get; set; } = default!;

        private readonly Action<StringBuilder> _delegate;

        public TemplateWithViewModel(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);
    }

    internal sealed class PlainTemplateWithAdditionalParameters
    {
        public string AdditionalParameter { get; set; } = "";

        public override string ToString() => AdditionalParameter;
    }

    internal sealed class PlainTemplateWithModelAndAdditionalParameters<T> : IModelContainer<T>
    {
        public T? Model { get; set; } = default!;

        public string AdditionalParameter { get; set; } = "";

        public override string ToString() => AdditionalParameter;
    }

    internal sealed class TextTransformTemplate : ITextTransformTemplate
    {
        private readonly Func<string> _delegate;

        public TextTransformTemplate(Func<string> @delegate) => _delegate = @delegate;

        public string TransformText() => _delegate();
    }

    internal sealed class MultipleContentBuilderTemplate : IMultipleContentBuilderTemplate
    {
        private readonly Action<IMultipleContentBuilder> _delegate;

        public MultipleContentBuilderTemplate(Action<IMultipleContentBuilder> @delegate) => _delegate = @delegate;

        public void Render(IMultipleContentBuilder builder) => _delegate(builder);
    }
}
