namespace TemplateFramework.Core.GenerationEnvironments;

internal sealed class MultipleContentBuilderEnvironment : GenerationEnvironmentBase
{
    public MultipleContentBuilderEnvironment(IMultipleContentBuilder builder) : base(GenerationEnvironmentType.MultipleContentBuilder)
    {
        Guard.IsNotNull(builder);

        Builder = builder;
    }

    public IMultipleContentBuilder Builder { get; }
}
