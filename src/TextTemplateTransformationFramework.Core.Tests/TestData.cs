namespace TemplateFramework.Core.Tests;

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

    internal sealed class TemplateWithViewModel<T> : ITemplate, IParameterizedTemplate
    {
        public T? ViewModel { get; set; } = default!;

        private readonly Action<StringBuilder> _delegate;

        public TemplateWithViewModel(Action<StringBuilder> @delegate) => _delegate = @delegate;

        public void Render(StringBuilder builder) => _delegate(builder);

        public void SetParameter(string name, object? value) // this is added in case of viewmodels which don't have a public parameterless constructor
        {
            if (name == nameof(ViewModel))
            {
                ViewModel = (T?)value;
            }
        }
    }

    internal sealed class PlainTemplateWithAdditionalParameters : IParameterizedTemplate
    {
        public string AdditionalParameter { get; set; } = "";

        public void SetParameter(string name, object? value)
        {
            if (name == nameof(AdditionalParameter))
            {
                AdditionalParameter = value?.ToString() ?? string.Empty;
            }
        }

        public override string ToString() => AdditionalParameter;
    }

    internal sealed class PlainTemplateWithModelAndAdditionalParameters<T> : IModelContainer<T>, IParameterizedTemplate
    {
        public T? Model { get; set; } = default!;

        public string AdditionalParameter { get; set; } = "";

        public void SetParameter(string name, object? value)
        {
            if (name == nameof(AdditionalParameter))
            {
                AdditionalParameter = value?.ToString() ?? string.Empty;
            }
        }

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

    internal sealed class MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine : IMultipleContentBuilderTemplate, ITemplateContextContainer, ITemplateEngineContainer
    {
        private readonly Action<IMultipleContentBuilder, ITemplateContext, ITemplateEngine, IChildTemplateFactory> _delegate;

        public MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(IChildTemplateFactory childTemplateFactory,
                                                                                  Action<IMultipleContentBuilder, ITemplateContext, ITemplateEngine, IChildTemplateFactory> @delegate)
        {
            ChildTemplateFactory = childTemplateFactory;
            _delegate = @delegate;
        }

        public ITemplateContext Context { get; set; } = default!;
        public ITemplateEngine TemplateEngine { get; set; } = default!;
        public IChildTemplateFactory ChildTemplateFactory { get; }

        public void Render(IMultipleContentBuilder builder) => _delegate(builder, Context, TemplateEngine, ChildTemplateFactory);
    }

    internal sealed class ViewModel : IParameterizedTemplate
    {
        public string? AdditionalParameter { get; set; }
        public string Property { get; set; } = "";
        public string ReadOnlyParameter => "Original value";
        public TestEnum EnumParameter { get; set; }

        public void SetParameter(string name, object? value)
        {
            switch (name)
            {
                case nameof(AdditionalParameter):
                    AdditionalParameter = value?.ToString();
                    break;
                case nameof(Property):
                    Property = value?.ToString() ?? string.Empty;
                    break;
                case nameof(EnumParameter):
                    EnumParameter = value is string s ? Enum.Parse<TestEnum>(s) : (TestEnum)Convert.ToInt32(value);
                    break;
            }
        }
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
    internal sealed class NonConstructableViewModel : IParameterizedTemplate
    {
        public NonConstructableViewModel(string property)
        {
            Property = property;
        }

        public string Property { get; set; }

        public void SetParameter(string name, object? value)
        {
            if (name == nameof(Property))
            {
                Property = value?.ToString() ?? string.Empty;
            }
        }
    }

    internal static string GetAssemblyName() => typeof(TestData).Assembly.FullName!;
}

public sealed class MyGeneratorProvider : ICodeGenerationProvider
{
    public bool GenerateMultipleFiles { get; private set; }

    public bool SkipWhenFileExists { get; private set; }

    public string BasePath { get; private set; } = "";

    public string Path { get; } = "";

    public string DefaultFilename => "MyFile.txt";

    public bool RecurseOnDeleteGeneratedFiles { get; }

    public string LastGeneratedFilesFilename { get; } = "";

    public object? CreateAdditionalParameters() => null;

    public object CreateGenerator() => new MyGenerator();

    public object? CreateModel() => null;

    public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath)
    {
        GenerateMultipleFiles = generateMultipleFiles;
        SkipWhenFileExists = skipWhenFileExists;
        BasePath = basePath;
    }
}

public sealed class MyGenerator
{
    public override string ToString() => "Here is code from MyGenerator";
}
