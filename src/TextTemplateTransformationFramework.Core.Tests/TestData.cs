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
        internal string InternalParameter { get; set; } = "";
        public string ReadOnlyParameter => "Original value";

        public override string ToString() => AdditionalParameter;
    }

    internal sealed class PlainTemplateWithModelAndAdditionalParameters<T> : IModelContainer<T>
    {
        public T? Model { get; set; } = default!;

        public string AdditionalParameter { get; set; } = "";

        public override string ToString() => AdditionalParameter;
    }

    internal sealed class PlainTemplateWithTemplateContext : ITemplateContextContainer
    {
        private readonly Func<ITemplateContext, string> _delegate;

        public PlainTemplateWithTemplateContext(Func<ITemplateContext, string> @delegate) => _delegate = @delegate;

        public ITemplateContext Context { get; set; } = default!;

        public override string ToString() => _delegate(Context);
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

    internal sealed class MultipleContentBuilderTemplateWithTepmlateContext : IMultipleContentBuilderTemplate, ITemplateContextContainer
    {
        private readonly Action<IMultipleContentBuilder, ITemplateContext> _delegate;

        public MultipleContentBuilderTemplateWithTepmlateContext(Action<IMultipleContentBuilder, ITemplateContext> @delegate) => _delegate = @delegate;

        public ITemplateContext Context { get; set; } = default!;

        public void Render(IMultipleContentBuilder builder) => _delegate(builder, Context);
    }

    internal sealed class ViewModel
    {
        public string AdditionalParameter { get; set; } = "";
        public string Property { get; set; } = "";
        public string ReadOnlyParameter => "Original value";
        public TestEnum EnumParameter { get; set; }
    }

    internal sealed class ViewModelWithModel<T> : IModelContainer<T>
    {
        public T? Model { get; set; }
    }

    internal sealed class ViewModelWithTemplateContext : ITemplateContextContainer
    {
        public ITemplateContext Context { get; set; } = default!;
    }

    /// <summary>
    /// Example of a ViewModel class without a public parameterless constructor. This one can't be initialized by the TemplateInitializer.
    /// </summary>
    internal sealed class NonConstructableViewModel
    {
        public NonConstructableViewModel(string property)
        {
            Property = property;
        }

        public string Property { get; set; }
    }
}
